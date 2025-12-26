using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Domain.Entities.Base;
using TradeSuite.Infrastructure.Database;

using MongoDB.Driver;

using System.Reflection;

namespace TradeSuite.Infrastructure.Repositories;

public class Repository<T>(MongoDbContext dbContext) : IRepository<T> where T : BaseEntity
{
    private readonly IMongoCollection<T> _collection = dbContext.GetCollection<T>($"{typeof(T).Name}s");

    public async Task<Guid> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        PropertyInfo? idProperty = typeof(T).GetProperty("Id");
        
        if (idProperty != null && idProperty.PropertyType == typeof(Guid))
        {
            return (Guid)idProperty.GetValue(entity)!;
        }
        
        throw new InvalidOperationException("Entity does not have a valid Id property of type Guid.");
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        FilterDefinition<T> filter = Builders<T>.Filter.Eq(x => x.Id, id);

        await _collection.DeleteOneAsync(filter, cancellationToken);
    }

    public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        FilterDefinition<T> filter = Builders<T>.Filter.Eq(x => x.IsActive, true) &
            Builders<T>.Filter.Eq(x => x.IsDeleted, false);

        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByFilterAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        FilterDefinition<T> filter = Builders<T>.Filter.Eq(x => x.Id, id) &
            Builders<T>.Filter.Eq(x => x.IsActive, true) &
            Builders<T>.Filter.Eq(x => x.IsDeleted, false);

        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        var update = Builders<T>.Update
            .Set(x => x.IsActive, false)
            .Set(x => x.IsDeleted, true);

        var options = new FindOneAndUpdateOptions<T, T>
        {
            ReturnDocument = ReturnDocument.After
        };

        return await _collection.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
    }

    public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        PropertyInfo? idProperty = typeof(T).GetProperty("Id");
        
        if (idProperty == null || idProperty.PropertyType != typeof(Guid))
        {
            throw new InvalidOperationException("Entity does not have a valid Id property of type Guid.");
        }

        Guid id = (Guid)idProperty.GetValue(entity)!;
        FilterDefinition<T> filter = Builders<T>.Filter.Eq(x => x.Id, id);

        ReplaceOneResult replaceOneResult = await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);

        return replaceOneResult.ModifiedCount == 1;
    }
}
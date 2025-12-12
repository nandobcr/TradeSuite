using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Infrastructure.Database;

using MongoDB.Driver;

namespace TradeSuite.Infrastructure.Repositories;

public class AuditRepository<T>(MongoDbContext dbContext) : IAuditRepository<T> where T : class
{
    private readonly IMongoCollection<T> _collection = dbContext
        .GetCollection<T>($"Audit_{typeof(T).GenericTypeArguments[0].Name}s");
    
    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }
}
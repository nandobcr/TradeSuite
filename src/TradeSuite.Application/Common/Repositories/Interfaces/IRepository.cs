using TradeSuite.Domain.Entities.Base;

using MongoDB.Driver;

namespace TradeSuite.Application.Common.Repositories.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<Guid> CreateAsync(T entity, CancellationToken cancellationToken = default);
    
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<T?> GetByFilterAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IList<T>> GetManyByFilterAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    Task<T?> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
}
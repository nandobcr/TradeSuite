namespace TradeSuite.Application.Common.Repositories.Interfaces;

public interface IAuditRepository<T>
{
    Task CreateAsync(T auditEntity, CancellationToken cancellationToken = default);
}
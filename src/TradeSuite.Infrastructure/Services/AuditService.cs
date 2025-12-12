using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.Common.Services.Interfaces;
using TradeSuite.Domain.Entities.Audit;
using TradeSuite.Domain.Entities.Base;

namespace TradeSuite.Infrastructure.Services;

public class AuditService<T>(IAuditRepository<AuditLog<T>> auditRepository) : IAuditService<T> where T : BaseEntity
{
    public Task LogAsync(Guid entityId, T entity, string operation, string changedBy)
    {
        var auditLog = new AuditLog<T>
        {
            EntityId = entityId,
            EntityData = entity,
            Operation = operation,
            ChangedBy = changedBy,
            Timestamp = DateTime.UtcNow
        };

        return auditRepository.CreateAsync(auditLog);
    }
}
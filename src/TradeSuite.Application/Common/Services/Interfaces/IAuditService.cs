using TradeSuite.Domain.Entities.Base;

namespace TradeSuite.Application.Common.Services.Interfaces;

public interface IAuditService<T> where T : BaseEntity
{
    Task LogAsync(Guid entityId, T entity, string operation, string changedBy);
}

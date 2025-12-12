using TradeSuite.Domain.Common.Interfaces;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities.Base;

public abstract class BaseEntity(IDateTimeProvider dateTimeProvider)
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public bool Active { get; set; } = true;

    public DateTime CreatedAt { get; set; } = dateTimeProvider.UtcNow;

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? UpdatedAt { get; set; } = null;

    public void SoftDelete(string user)
    {
        IsDeleted = true;
        DeletedAt = dateTimeProvider.UtcNow;
        DeletedBy = user;
        Active = false;
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities.Base;

public abstract class BaseEntity(DateTime utcNow)
{
    [BsonId]
    [BsonRepresentation(BsonType.String)] 
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = utcNow;

    public required string CreatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    public DateTime? UpdatedAt { get; set; } = null;

    public string UpdatedBy { get; set; } = string.Empty;

    public void SoftDelete(string user)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = user;
        IsActive = false;
    }
}
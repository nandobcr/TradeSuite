using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities.Audit;

public class AuditLog<T>
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonRepresentation(BsonType.String)]
    public Guid EntityId { get; set; }

    public T EntityData { get; set; } = default!;

    public string Operation { get; set; } = string.Empty;

    public string ChangedBy { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }
}
using TradeSuite.Domain.Entities.Base;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities;

public class SupplierPriceListItem(DateTime utcNow) : BaseEntity(utcNow)
{
    public string Currency { get; set; } = "EUR";

    public required string Reference { get; set; }

    [BsonRepresentation(BsonType.String)]
    public required Guid SupplierPartId { get; set; }

    public decimal UnitPrice { get; set; } = 0m;

    public DateTime ValidFrom { get; set; } = utcNow;

    public DateTime? ValidUntil { get; set; } = null;
}
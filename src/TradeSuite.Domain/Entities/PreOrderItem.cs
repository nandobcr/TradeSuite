using TradeSuite.Domain.Entities.Base;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities;

public class PreOrderItem(DateTime utcNow) : BaseEntity(utcNow)
{
    public int Quantity { get; set; }

    public required string Reference { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid SupplierPartId { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid SupplierPriceListItemId { get; set; }

    public decimal UnitCost { get; set; }

    public decimal UnitSale { get; set; }

    public decimal TotalCost { get; set; }

    public decimal TotalSale { get; set; }
}
using TradeSuite.Domain.Entities.Base;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities;

public class OrderItem(DateTime utcNow) : BaseEntity(utcNow)
{
    public DateTime ExpectedDeliveryDate { get; set; }

    public int Quantity { get; set; } = 0;

    public required string Reference { get; set; }
    
    public DateTime RequestDate { get; set; } = utcNow;

    [BsonRepresentation(BsonType.String)]
    public Guid SupplierPartId { get; set; }

    public decimal TotalCost { get; set; }

    public decimal TotalSale { get; set; }

    public decimal UnitCost { get; set; } = 0m;

    public decimal UnitSale { get; set; } = 0m;
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities;

public class OrderSupplier
{
    public IList<OrderItem> OrderItems { get; set; } = [];
    
    [BsonRepresentation(BsonType.String)]
    public Guid SupplierId { get; set; }

    public required string SupplierName { get; set; }

    public decimal TotalCost { get; set; } = 0m;

    public decimal TotalSale { get; set; } = 0m;
}
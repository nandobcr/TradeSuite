using TradeSuite.Domain.Entities.Base;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities;

public class PreOrderSupplier(DateTime utcNow) : BaseEntity(utcNow)
{
    public IList<PreOrderItem> PreOrderItems { get; set; } = [];

    [BsonRepresentation(BsonType.String)]
    public Guid SupplierId { get; set; }

    public required string SupplierName { get; set; }

    public decimal TotalCost { get; set; }

    public decimal TotalSale { get; set; }
}
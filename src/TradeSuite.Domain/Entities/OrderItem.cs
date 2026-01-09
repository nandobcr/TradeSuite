using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities.Base;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities;

public class OrderItem(IDateTimeProvider dateTimeProvider) : BaseEntity(dateTimeProvider)
{
    [BsonRepresentation(BsonType.String)]
    public Guid SupplierPartId { get; set; }

    public int Quantity { get; set; } = 0;

    public decimal UnitPrice { get; set; } = 0m;

    public decimal TotalPrice => UnitPrice > 0 ? UnitPrice * Quantity : UnitPrice;

    public DateTime RequestDate { get; set; } = dateTimeProvider.UtcNow;

    public DateTime ExpectedDeliveryDate { get; set; }
}
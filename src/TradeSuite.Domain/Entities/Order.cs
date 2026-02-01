using TradeSuite.Domain.Common.Enums;
using TradeSuite.Domain.Entities.Base;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities;

public class Order(DateTime utcNow) : BaseEntity(utcNow)
{
    [BsonRepresentation(BsonType.String)]
    public Guid ClientId { get; set; }

    public decimal MarginPercentage { get; set; } = 0m;

    public DateTime OrderDate { get; set; } = utcNow;

    public IList<OrderSupplier> OrderSuppliers { get; set; } = [];

    public string Status { get; set; } = OrderStatuses.Pending.ToString();

    public decimal TotalCost { get; set; } = 0m;

    public decimal TotalSale { get; set; } = 0m;

    public void Cancel()
    {
        Status = OrderStatuses.Cancelled.ToString();
    }
}
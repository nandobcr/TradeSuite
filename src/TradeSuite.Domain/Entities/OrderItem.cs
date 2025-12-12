using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities.Base;

namespace TradeSuite.Domain.Entities;

public class OrderItem(IDateTimeProvider dateTimeProvider) : BaseEntity(dateTimeProvider)
{
    public string SupplierPartId { get; set; } = string.Empty;

    public int Quantity { get; set; } = 0;

    public decimal UnitPrice { get; set; } = 0m;

    public decimal TotalPrice => UnitPrice > 0 ? UnitPrice * Quantity : UnitPrice;

    public DateTime RequestDate { get; set; } = dateTimeProvider.UtcNow;

    public DateTime ExpectedDeliveryDate { get; set; }
}
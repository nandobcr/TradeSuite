using TradeSuite.Domain.Common.Enums;
using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities.Base;

namespace TradeSuite.Domain.Entities;

public class Order(IDateTimeProvider dateTimeProvider) : BaseEntity(dateTimeProvider)
{
    public string ClientId { get; set; } = string.Empty;

    public IList<OrderItem> Items { get; set; } = [];

    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);

    public DateTime OrderDate { get; set; } = dateTimeProvider.UtcNow;

    public string Status { get; set; } = OrderStatuses.Pending.ToString();
}
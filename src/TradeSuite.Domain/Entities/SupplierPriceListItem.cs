using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities.Base;

namespace TradeSuite.Domain.Entities;

public class SupplierPriceListItem(IDateTimeProvider dateTimeProvider) : BaseEntity(dateTimeProvider)
{
    public string SupplierPartId { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; } = 0m;

    public string Currency { get; set; } = string.Empty;

    public DateTime ValidFrom { get; set; }

    public DateTime ValidUntil { get; set; }
}
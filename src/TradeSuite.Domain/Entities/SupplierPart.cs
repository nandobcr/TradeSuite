using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities.Base;

namespace TradeSuite.Domain.Entities;

public class SupplierPart(IDateTimeProvider dateTimeProvider) : BaseEntity(dateTimeProvider)
{
    public string SupplierId { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int LeadTimeDays { get; set; } = 0;
}
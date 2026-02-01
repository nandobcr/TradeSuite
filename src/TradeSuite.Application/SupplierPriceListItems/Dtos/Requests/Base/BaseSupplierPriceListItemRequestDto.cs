namespace TradeSuite.Application.SupplierPriceListItems.Dtos.Requests.Base;

public class BaseSupplierPriceListItemRequestDto
{
    public string Currency { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;

    public Guid SupplierPartId { get; set; }

    public decimal UnitPrice { get; set; } = 0m;

    public DateTime ValidFrom { get; set; }
}

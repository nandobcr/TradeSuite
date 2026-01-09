using TradeSuite.Application.SupplierPriceListItems.Dtos.Requests.Base;

namespace TradeSuite.Application.SupplierPriceListItems.Dtos.Requests;

public class UpdateSupplierPriceListItemRequestDto : BaseSupplierPriceListItemRequestDto
{
    public bool IsActive { get; set; } = true;
}
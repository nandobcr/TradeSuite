using TradeSuite.Application.SupplierPriceListItems.Dtos.Requests;
using TradeSuite.Application.SupplierPriceListItems.Dtos.Responses;

namespace TradeSuite.Application.SupplierPriceListItems.Interfaces;

public interface ISupplierPriceListItemService
{
    Task<CreateSupplierPriceListItemResponseDto> CreateSupplierPriceListItemAsync(CreateSupplierPriceListItemRequestDto createSupplierPriceListItemRequestDto);
    
    Task DeleteSupplierPriceListItemAsync(Guid supplierPriceListItemId);
    
    Task<IList<SupplierPriceListItemResponseDto>> GetAllSupplierPriceListItemsAsync();
    
    Task<SupplierPriceListItemResponseDto> GetSupplierPriceListItemByIdAsync(Guid supplierPriceListItemId);
}
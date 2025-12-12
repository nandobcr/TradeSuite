using TradeSuite.Application.Suppliers.Dtos.Requests;
using TradeSuite.Application.Suppliers.Dtos.Responses;

namespace TradeSuite.Application.Suppliers.Interfaces;

public interface ISupplierService
{
    Task<CreateSupplierResponseDto> CreateSupplierAsync(CreateSupplierRequestDto createSupplierRequestDto);
    
    Task DeleteSupplierAsync(Guid supplierId);

    Task<UpdateSupplierResponseDto> UpdateSupplierAsync(Guid supplierId, UpdateSupplierRequestDto updateSupplierDto);
    
    Task<IList<SupplierResponseDto>> GetAllSuppliersAsync();
    
    Task<SupplierResponseDto> GetSupplierByIdAsync(Guid supplierId);
}

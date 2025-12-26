using TradeSuite.Application.SupplierParts.Dtos.Requests;
using TradeSuite.Application.SupplierParts.Dtos.Responses;

namespace TradeSuite.Application.SupplierParts.Interfaces;

public interface ISupplierPartService
{
    Task<CreateSupplierPartResponseDto> CreateSupplierPartAsync(CreateSupplierPartRequestDto createSupplierPartRequestDto);
    
    Task DeleteSupplierPartAsync(Guid supplierPartId);
    
    Task<IList<SupplierPartResponseDto>> GetAllSupplierPartsAsync();
    
    Task<SupplierPartResponseDto> GetSupplierPartByIdAsync(Guid supplierPartId);

    Task<UpdateSupplierPartResponseDto> UpdateSupplierPartAsync(Guid supplierPartId, UpdateSupplierPartRequestDto updateSupplierPartDto);
}
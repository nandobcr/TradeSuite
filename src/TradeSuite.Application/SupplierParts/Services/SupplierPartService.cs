using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.Common.Services.Interfaces;
using TradeSuite.Application.SupplierParts.Dtos.Requests;
using TradeSuite.Application.SupplierParts.Dtos.Responses;
using TradeSuite.Application.SupplierParts.Helpers;
using TradeSuite.Application.SupplierParts.Interfaces;
using TradeSuite.Domain.Common.Enums;
using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities;

namespace TradeSuite.Application.SupplierParts.Services;

public class SupplierPartService(
    IDateTimeProvider dateTimeProvider,
    IRepository<Supplier> supplierRepository,
    IRepository<SupplierPart> supplierPartRepository,
    IAuditService<SupplierPart> auditService) : ISupplierPartService
{
    public async Task<CreateSupplierPartResponseDto> CreateSupplierPartAsync(CreateSupplierPartRequestDto createSupplierPartRequestDto)
    {
        Supplier? supplier = await supplierRepository.GetByIdAsync(createSupplierPartRequestDto.SupplierId);
        SupplierPartValidator.ValidateSupplierPartRequestDto(supplier, createSupplierPartRequestDto);

        SupplierPart supplierPart = new(dateTimeProvider.UtcNow)
        {
            CreatedBy = "user logado",
            Description = createSupplierPartRequestDto.Description,
            Reference = createSupplierPartRequestDto.Reference,
            SupplierId = createSupplierPartRequestDto.SupplierId
        };

        Guid id = await supplierPartRepository.CreateAsync(supplierPart);

        if (id != Guid.Empty)
        {
            await auditService.LogAsync(id, supplierPart, OperationTypes.Create.ToString(), "user logado");
        }

        return new CreateSupplierPartResponseDto { Id = id };
    }

    public async Task DeleteSupplierPartAsync(Guid supplierPartId)
    {
        _ = await supplierPartRepository.GetByIdAsync(supplierPartId)
            ?? throw new KeyNotFoundException($"Supplier part with ID {supplierPartId} not found.");

        SupplierPart? updatedSupplierPart = await supplierPartRepository.SoftDeleteAsync(supplierPartId);

        if (updatedSupplierPart != null && updatedSupplierPart.IsDeleted)
        {
            await auditService.LogAsync(supplierPartId, updatedSupplierPart, OperationTypes.SoftDelete.ToString(), "user logado");
        }
    }

    public async Task<IList<SupplierPartResponseDto>> GetAllSupplierPartsAsync()
    {
        IList<SupplierPart> supplierParts = await supplierPartRepository.GetAllAsync();

        return [.. supplierParts.Select(supplierPart => new SupplierPartResponseDto
        {
            Id = supplierPart.Id,
            Description = supplierPart.Description,
            IsActive = supplierPart.IsActive,
            IsDeleted = supplierPart.IsDeleted,
            Reference = supplierPart.Reference,
            SupplierId = supplierPart.SupplierId
        })];
    }

    public async Task<SupplierPartResponseDto> GetSupplierPartByIdAsync(Guid supplierPartId)
    {
        SupplierPart? supplierPart = await supplierPartRepository.GetByIdAsync(supplierPartId) 
            ?? throw new KeyNotFoundException($"Supplier part with ID {supplierPartId} not found.");

        return new SupplierPartResponseDto
        {
            Id = supplierPart.Id,
            Description = supplierPart.Description,
            IsActive = supplierPart.IsActive,
            IsDeleted = supplierPart.IsDeleted,
            Reference = supplierPart.Reference,
            SupplierId = supplierPart.SupplierId
        };
    }

    public async Task<UpdateSupplierPartResponseDto> UpdateSupplierPartAsync(Guid supplierPartId, UpdateSupplierPartRequestDto updateSupplierPartDto)
    {
        Supplier? supplier = await supplierRepository.GetByIdAsync(updateSupplierPartDto.SupplierId);
        SupplierPartValidator.ValidateSupplierPartRequestDto(supplier, updateSupplierPartDto);

        SupplierPart? supplierPart = await supplierPartRepository.GetByIdAsync(supplierPartId)
            ?? throw new KeyNotFoundException($"Supplier part with ID {supplierPartId} not found.");

        supplierPart.Description = !string.IsNullOrWhiteSpace(updateSupplierPartDto.Description) 
            ? updateSupplierPartDto.Description : supplierPart.Description;
        supplierPart.IsActive = updateSupplierPartDto.IsActive;
        supplierPart.Reference =  !string.IsNullOrWhiteSpace(updateSupplierPartDto.Reference) 
            ? updateSupplierPartDto.Reference : supplierPart.Reference;
        supplierPart.SupplierId = updateSupplierPartDto.SupplierId;
        supplierPart.UpdatedAt = dateTimeProvider.UtcNow;

        bool isUpdated = await supplierPartRepository.UpdateAsync(supplierPart);
        if (isUpdated)
        {
            await auditService.LogAsync(supplierPartId, supplierPart, OperationTypes.Update.ToString(), "user logado");
        }

        return new UpdateSupplierPartResponseDto
        {
            Id = supplierPartId,
            Description = supplierPart.Description,
            IsActive = supplierPart.IsActive,
            IsDeleted = supplierPart.IsDeleted,
            Reference = supplierPart.Reference,
            SupplierId = supplierPart.SupplierId
        };
    }
}
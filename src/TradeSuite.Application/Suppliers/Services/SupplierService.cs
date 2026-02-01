using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.Common.Services.Interfaces;
using TradeSuite.Application.Suppliers.Dtos.Requests;
using TradeSuite.Application.Suppliers.Dtos.Responses;
using TradeSuite.Application.Suppliers.Helpers;
using TradeSuite.Application.Suppliers.Interfaces;
using TradeSuite.Domain.Common.Enums;
using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities;

namespace TradeSuite.Application.Suppliers.Services;

public class SupplierService(
    IDateTimeProvider dateTimeProvider,
    IRepository<Supplier> supplierRepository,
    IAuditService<Supplier> auditService) : ISupplierService
{
    public async Task<CreateSupplierResponseDto> CreateSupplierAsync(CreateSupplierRequestDto createSupplierRequestDto)
    {
        SupplierValidator.ValidateSupplierRequestDto(createSupplierRequestDto);

        Supplier supplier = new(dateTimeProvider.UtcNow)
        {
            Address = createSupplierRequestDto.Address,
            CreatedBy = "user logado",
            Email = createSupplierRequestDto.Email,
            Name = createSupplierRequestDto.Name,
            Phone = createSupplierRequestDto.Phone
        };

        Guid id = await supplierRepository.CreateAsync(supplier);

        if (id != Guid.Empty)
        {
            await auditService.LogAsync(id, supplier, OperationTypes.Create.ToString(), "user logado");
        }

        return new CreateSupplierResponseDto { Id = id };
    }

    public async Task DeleteSupplierAsync(Guid supplierId)
    {
        _ = await supplierRepository.GetByIdAsync(supplierId)
            ?? throw new KeyNotFoundException($"Supplier with ID {supplierId} not found.");

        Supplier? updatedSupplier = await supplierRepository.SoftDeleteAsync(supplierId);

        if (updatedSupplier != null && updatedSupplier.IsDeleted)
        {
            await auditService.LogAsync(supplierId, updatedSupplier, OperationTypes.SoftDelete.ToString(), "user logado");
        }
    }

    public async Task<IList<SupplierResponseDto>> GetAllSuppliersAsync()
    {
        IList<Supplier> suppliers = await supplierRepository.GetAllAsync();

        return [.. suppliers.Select(supplier => new SupplierResponseDto
        {
            Id = supplier.Id,
            Address = supplier.Address,
            Email = supplier.Email,
            IsActive = supplier.IsActive,
            IsDeleted = supplier.IsDeleted,
            Name = supplier.Name,
            Phone = supplier.Phone
        })];
    }

    public async Task<SupplierResponseDto> GetSupplierByIdAsync(Guid supplierId)
    {
        Supplier? supplier = await supplierRepository.GetByIdAsync(supplierId)
            ?? throw new KeyNotFoundException($"Supplier with ID {supplierId} not found.");

        return new SupplierResponseDto
        {
            Id = supplier.Id,
            Address = supplier.Address,
            Email = supplier.Email,
            IsActive = supplier.IsActive,
            IsDeleted = supplier.IsDeleted,
            Name = supplier.Name,
            Phone = supplier.Phone
        };
    }

    public async Task<UpdateSupplierResponseDto> UpdateSupplierAsync(Guid supplierId, UpdateSupplierRequestDto updateSupplierDto)
    {
        SupplierValidator.ValidateSupplierRequestDto(updateSupplierDto);

        Supplier supplier = await supplierRepository.GetByIdAsync(supplierId)
            ?? throw new KeyNotFoundException($"Supplier with ID {supplierId} not found.");

        supplier.Address = !string.IsNullOrWhiteSpace(updateSupplierDto.Address) ? updateSupplierDto.Address : supplier.Address;
        supplier.Email = updateSupplierDto.Email;
        supplier.IsActive = updateSupplierDto.IsActive;
        supplier.Name = updateSupplierDto.Name;
        supplier.Phone = !string.IsNullOrWhiteSpace(updateSupplierDto.Phone) ? updateSupplierDto.Phone : supplier.Phone;
        supplier.UpdatedAt = dateTimeProvider.UtcNow;

        bool isUpdated = await supplierRepository.UpdateAsync(supplier);
        if (isUpdated)
        {
            await auditService.LogAsync(supplierId, supplier, OperationTypes.Update.ToString(), "user logado");
        }

        return new UpdateSupplierResponseDto
        {
            Id = supplier.Id,
            Address = supplier.Address,
            Email = supplier.Email,
            IsActive = supplier.IsActive,
            IsDeleted = supplier.IsDeleted,
            Name = supplier.Name,
            Phone = supplier.Phone
        };
    }
}
using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.Common.Services.Interfaces;
using TradeSuite.Application.SupplierPriceListItems.Dtos.Requests;
using TradeSuite.Application.SupplierPriceListItems.Dtos.Responses;
using TradeSuite.Application.SupplierPriceListItems.Helpers;
using TradeSuite.Application.SupplierPriceListItems.Interfaces;
using TradeSuite.Domain.Common.Enums;
using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities;

using MongoDB.Driver;

namespace TradeSuite.Application.SupplierPriceListItems.Services;

public class SupplierPriceListItemService(
    IDateTimeProvider dateTimeProvider,
    IRepository<SupplierPart> supplierPartRepository,
    IRepository<SupplierPriceListItem> supplierPriceListItemRepository,
    IAuditService<SupplierPriceListItem> auditService) : ISupplierPriceListItemService
{
    public async Task<CreateSupplierPriceListItemResponseDto> CreateSupplierPriceListItemAsync(
        CreateSupplierPriceListItemRequestDto createSupplierPriceListItemRequestDto)
    {
        SupplierPart? supplierPart = await supplierPartRepository.GetByIdAsync(createSupplierPriceListItemRequestDto.SupplierPartId);
        SupplierPriceListItemValidator.ValidateSupplierPriceListItemRequestDto(supplierPart, createSupplierPriceListItemRequestDto);

        FilterDefinition<SupplierPriceListItem> filter =
            Builders<SupplierPriceListItem>.Filter.Eq(x => x.SupplierPartId, createSupplierPriceListItemRequestDto.SupplierPartId) &
            Builders<SupplierPriceListItem>.Filter.Lte(x => x.ValidFrom, dateTimeProvider.UtcNow) &
            Builders<SupplierPriceListItem>.Filter.Or(
                Builders<SupplierPriceListItem>.Filter.Eq(x => x.ValidUntil, null),
                Builders<SupplierPriceListItem>.Filter.Gt(x => x.ValidUntil, dateTimeProvider.UtcNow)
            ) &
            Builders<SupplierPriceListItem>.Filter.Eq(x => x.IsActive, true) &
            Builders<SupplierPriceListItem>.Filter.Eq(x => x.IsDeleted, false);

        SupplierPriceListItem? currentSupplierPriceListItem = await supplierPriceListItemRepository.GetByFilterAsync(filter);

        if (currentSupplierPriceListItem is not null)
        {
            currentSupplierPriceListItem.ValidUntil = createSupplierPriceListItemRequestDto.ValidFrom;
            await supplierPriceListItemRepository.UpdateAsync(currentSupplierPriceListItem);
            await auditService.LogAsync(currentSupplierPriceListItem.Id, currentSupplierPriceListItem, OperationTypes.Update.ToString(), "user logado");
        }

        SupplierPriceListItem supplierPriceListItem = new(dateTimeProvider.UtcNow)
        {
            CreatedBy = "user logado",
            Currency = createSupplierPriceListItemRequestDto.Currency,
            Reference = createSupplierPriceListItemRequestDto.Reference,
            SupplierPartId = createSupplierPriceListItemRequestDto.SupplierPartId,
            UnitPrice = createSupplierPriceListItemRequestDto.UnitPrice,
            ValidFrom = createSupplierPriceListItemRequestDto.ValidFrom,
        };

        Guid id = await supplierPriceListItemRepository.CreateAsync(supplierPriceListItem);
        
        if (id != Guid.Empty)
        {
            await auditService.LogAsync(id, supplierPriceListItem, OperationTypes.Create.ToString(), "user logado");
        }

        return new CreateSupplierPriceListItemResponseDto { Id = id };
    }

    public async Task DeleteSupplierPriceListItemAsync(Guid supplierPriceListItemId)
    {
        _ = await supplierPriceListItemRepository.GetByIdAsync(supplierPriceListItemId)
            ?? throw new KeyNotFoundException($"Supplier price list item with ID {supplierPriceListItemId} not found.");

        SupplierPriceListItem? updatedSupplierPriceListItem = await supplierPriceListItemRepository.SoftDeleteAsync(supplierPriceListItemId);

        if (updatedSupplierPriceListItem != null && updatedSupplierPriceListItem.IsDeleted)
        {
            await auditService.LogAsync(supplierPriceListItemId, updatedSupplierPriceListItem, OperationTypes.SoftDelete.ToString(), "user logado");
        }
    }

    public async Task<IList<SupplierPriceListItemResponseDto>> GetAllSupplierPriceListItemsAsync()
    {
        IList<SupplierPriceListItem> supplierPriceListItems = await supplierPriceListItemRepository.GetAllAsync(); 

        return [.. supplierPriceListItems.Select(supplierPriceListItem => new SupplierPriceListItemResponseDto
        {
            Id = supplierPriceListItem.Id,
            Currency = supplierPriceListItem.Currency,
            IsActive = supplierPriceListItem.IsActive,
            IsDeleted = supplierPriceListItem.IsDeleted,
            SupplierPartId = supplierPriceListItem.SupplierPartId,
            UnitPrice = supplierPriceListItem.UnitPrice,
            ValidFrom = supplierPriceListItem.ValidFrom,
            ValidUntil = supplierPriceListItem.ValidUntil
        })];
    }

    public async Task<SupplierPriceListItemResponseDto> GetSupplierPriceListItemByIdAsync(Guid supplierPriceListItemId)
    {
        SupplierPriceListItem? supplierPriceListItem = await supplierPriceListItemRepository.GetByIdAsync(supplierPriceListItemId) 
            ?? throw new KeyNotFoundException($"Supplier price list item with ID {supplierPriceListItemId} not found.");

        return new SupplierPriceListItemResponseDto
        {  
            Id = supplierPriceListItem.Id,
            Currency = supplierPriceListItem.Currency,
            IsActive = supplierPriceListItem.IsActive,
            IsDeleted = supplierPriceListItem.IsDeleted,
            SupplierPartId = supplierPriceListItem.SupplierPartId,
            UnitPrice = supplierPriceListItem.UnitPrice,
            ValidFrom = supplierPriceListItem.ValidFrom,
            ValidUntil = supplierPriceListItem.ValidUntil
        };
    }
}
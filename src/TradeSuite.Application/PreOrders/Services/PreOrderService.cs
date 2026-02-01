using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.PreOrders.Dtos.Requests;
using TradeSuite.Application.PreOrders.Dtos.Responses;
using TradeSuite.Application.PreOrders.Helpers;
using TradeSuite.Application.PreOrders.Interfaces;
using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities;

using MongoDB.Driver;

namespace TradeSuite.Application.PreOrders.Services;

public class PreOrderService(
    IDateTimeProvider dateTimeProvider,
    IRepository<Client> clientRepository,
    IRepository<PreOrder> preOrderRepository,
    IRepository<Supplier> supplierRepository,
    IRepository<SupplierPart> supplierPartRepository,
    IRepository<SupplierPriceListItem> supplierPriceListItemRepository) : IPreOrderService
{
    public async Task<CreatePreOrderResponseDto> CreatePreOrderAsync(CreatePreOrderRequestDto createPreOrderRequestDto)
    {
        Client? client = await clientRepository.GetByIdAsync(createPreOrderRequestDto.ClientId);
        PreOrderValidator.ValidatePreOrderRequestDto(client, createPreOrderRequestDto);

        var supplierBuckets = new Dictionary<Guid, PreOrderSupplier>();

        foreach (PreOrderItemRequestDto preOrderItemRequestDto in createPreOrderRequestDto.PreOrderItems)
        {
            FilterDefinition<SupplierPart> supplierPartFilter = Builders<SupplierPart>.Filter.Eq(x => x.Reference, preOrderItemRequestDto.Reference) &
                Builders<SupplierPart>.Filter.Eq(x => x.IsActive, true) &
                Builders<SupplierPart>.Filter.Eq(x => x.IsDeleted, false);

            IList<SupplierPart> supplierParts = await supplierPartRepository.GetManyByFilterAsync(supplierPartFilter);

            if (supplierParts.Count == 0)
            {
                continue;
            }

            foreach (SupplierPart supplierPart in supplierParts)
            {
                Supplier? supplier = await supplierRepository.GetByIdAsync(supplierPart.SupplierId);
                
                //ignore supplier parts from inactive or deleted suppliers
                if (supplier == null || !supplier.IsActive || supplier.IsDeleted)
                {
                    continue;
                }

                // get actual valid price
                FilterDefinition<SupplierPriceListItem> supplierPriceListItemFilter = Builders<SupplierPriceListItem>.Filter.Eq(x => x.SupplierPartId, supplierPart.Id) &
                    Builders<SupplierPriceListItem>.Filter.Lte(x => x.ValidFrom, dateTimeProvider.UtcNow) &
                    Builders<SupplierPriceListItem>.Filter.Or(
                        Builders<SupplierPriceListItem>.Filter.Eq(x => x.ValidUntil, null),
                        Builders<SupplierPriceListItem>.Filter.Gt(x => x.ValidUntil, dateTimeProvider.UtcNow)
                    ) &
                    Builders<SupplierPriceListItem>.Filter.Eq(x => x.IsActive, true) &
                    Builders<SupplierPriceListItem>.Filter.Eq(x => x.IsDeleted, false);

                SupplierPriceListItem? priceItem = await supplierPriceListItemRepository.GetByFilterAsync(supplierPriceListItemFilter);
                // ignore supplier parts without valid price
                if (priceItem == null)
                {   
                    continue;
                }

                decimal marginPercentage = createPreOrderRequestDto.MarginPercentage > 0
                    ? createPreOrderRequestDto.MarginPercentage
                    : preOrderItemRequestDto.MarginPercentage;
                
                decimal salePrice = priceItem.UnitPrice * (1 + marginPercentage / 100);

                if (!supplierBuckets.TryGetValue(supplierPart.SupplierId, out PreOrderSupplier? preOrderSupplier))
                {
                    preOrderSupplier = new PreOrderSupplier(dateTimeProvider.UtcNow)
                    {
                        CreatedBy = "user logado",
                        PreOrderItems = [],
                        SupplierId = supplier.Id,
                        SupplierName = supplier.Name,
                        TotalCost = 0,
                        TotalSale = 0
                    };
                    supplierBuckets.Add(supplierPart.SupplierId, preOrderSupplier);
                }

                var item = new PreOrderItem(dateTimeProvider.UtcNow)
                {
                    CreatedBy = "user logado",
                    Quantity = preOrderItemRequestDto.Quantity,
                    Reference = supplierPart.Reference,
                    SupplierPartId = supplierPart.Id,
                    UnitCost = priceItem.UnitPrice,
                    UnitSale = salePrice,
                    TotalCost = priceItem.UnitPrice * preOrderItemRequestDto.Quantity,
                    TotalSale = salePrice * preOrderItemRequestDto.Quantity
                };

                preOrderSupplier.PreOrderItems.Add(item);
                preOrderSupplier.TotalCost += item.TotalCost;
                preOrderSupplier.TotalSale += item.TotalSale;
            }    
        }

        var preOrder = new PreOrder(dateTimeProvider.UtcNow)
        {
            CreatedBy = "user logado",
            ClientId = createPreOrderRequestDto.ClientId,
            PreOrderSuppliers = [.. supplierBuckets.Values]
        };

        Guid preOrderId = await preOrderRepository.CreateAsync(preOrder);

        return new CreatePreOrderResponseDto
        {
            ClientId = createPreOrderRequestDto.ClientId,
            PreOrderId = preOrderId,
            PreOrderSuppliers = [.. supplierBuckets.Values.Select(x => new PreOrderSupplierResponseDto
            {
                SupplierId = x.SupplierId,
                SupplierName = x.SupplierName,
                TotalCost = x.TotalCost,
                TotalSale = x.TotalSale,
                PreOrderItems = [.. x.PreOrderItems.Select(item => new PreOrderItemResponseDto
                {
                    Reference = item.Reference,
                    Quantity = item.Quantity,
                    SupplierPartId = item.SupplierPartId,
                    UnitCost = item.UnitCost,
                    UnitSale = item.UnitSale,
                    TotalCost = item.TotalCost,
                    TotalSale = item.TotalSale
                })]
            })]
        };
    }
}
using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.Common.Services.Interfaces;
using TradeSuite.Application.Orders.Dtos.Requests;
using TradeSuite.Application.Orders.Dtos.Responses;
using TradeSuite.Application.Orders.Helpers;
using TradeSuite.Application.Orders.Interfaces;
using TradeSuite.Domain.Common.Enums;
using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities;

using MongoDB.Driver;

namespace TradeSuite.Application.Orders.Services;

public class OrderService(
    IDateTimeProvider dateTimeProvider,
    IRepository<Order> orderRepository,
    IRepository<PreOrder> preOrderRepository,
    IAuditService<Order> auditService) : IOrderService
{
    public async Task CancelOrderAsync(Guid orderId)
    {
        Order? order = await orderRepository.GetByIdAsync(orderId)
            ?? throw new KeyNotFoundException($"Order with Id {orderId} not found.");

        order.Cancel();
        
        bool isUpdated = await orderRepository.UpdateAsync(order);
        if (isUpdated)
        {
            await auditService.LogAsync(orderId, order, OperationTypes.Update.ToString(), "user logado");
        }
    }

    public async Task<CreateOrderResponseDto> CreateOrderFromPreOrderAsync(CreateOrderFromPreOrderRequestDto createOrderFromPreOrderRequestDto)
    {
        PreOrder? preOrder = await preOrderRepository.GetByIdAsync(createOrderFromPreOrderRequestDto.PreOrderId);
        OrderValidator.ValidateOrderRequestDto(preOrder, createOrderFromPreOrderRequestDto);

        Order order = new(dateTimeProvider.UtcNow)
        {
            ClientId = createOrderFromPreOrderRequestDto.ClientId,
            CreatedBy = "user logado",
            MarginPercentage = createOrderFromPreOrderRequestDto.MarginPercentage
        };

        foreach (CreateOrderSupplierSelectionRequestDto supplierSelection in createOrderFromPreOrderRequestDto.CreateOrderSupplierSelections)
        {
            PreOrderSupplier? preOrderSupplier = preOrder!.PreOrderSuppliers.FirstOrDefault(x => x.SupplierId == supplierSelection.SupplierId);
            OrderSupplierSelectionValidator.ValidateOrderSupplierSelection(preOrderSupplier);
            
            OrderSupplier orderSupplier = new()
            {
                SupplierId = supplierSelection.SupplierId,
                SupplierName = preOrderSupplier!.SupplierName
            };

            foreach (Guid supplierPartId in supplierSelection.SupplierPartIds)
            {
                PreOrderItem preOrderItem = preOrderSupplier!.PreOrderItems.First(x => x.SupplierPartId == supplierPartId);

                OrderItem orderItem = new(dateTimeProvider.UtcNow)
                {
                    CreatedBy = "user logado",
                    Quantity = preOrderItem.Quantity,
                    SupplierPartId = supplierPartId,
                    Reference = preOrderItem.Reference,
                    UnitCost = preOrderItem.UnitCost,
                    UnitSale = preOrderItem.UnitSale,
                    TotalCost = preOrderItem.Quantity * preOrderItem.UnitCost,
                    TotalSale = preOrderItem.Quantity * (preOrderItem.UnitSale * (createOrderFromPreOrderRequestDto.MarginPercentage / 100)),
                };

                orderSupplier.OrderItems.Add(orderItem);

                orderSupplier.TotalCost += orderItem.TotalCost;
                orderSupplier.TotalSale += orderItem.TotalSale;
            }

            order.OrderSuppliers.Add(orderSupplier);

            order.TotalCost += orderSupplier.TotalCost;
            order.TotalSale += orderSupplier.TotalSale;
        }

        await orderRepository.CreateAsync(order);

        //preOrder.MarkAsConfirmed();
        //await preOrderRepository.UpdateAsync(preOrder);


        // TODO: Send order confirmation email
        // TODO: Log audit
        // TODO: create entry points for preOrder and order
        // Test all steps and scenarios
        
        return new CreateOrderResponseDto { Id = order.Id };
    }
}
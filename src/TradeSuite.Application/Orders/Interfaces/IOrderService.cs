using TradeSuite.Application.Orders.Dtos.Requests;
using TradeSuite.Application.Orders.Dtos.Responses;

namespace TradeSuite.Application.Orders.Interfaces;

public interface IOrderService
{
    Task CancelOrderAsync(Guid orderId);
    
    Task<CreateOrderResponseDto> CreateOrderFromPreOrderAsync(CreateOrderFromPreOrderRequestDto createOrderFromPreOrderRequestDto);
}
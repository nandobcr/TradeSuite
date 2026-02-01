using TradeSuite.Application.PreOrders.Dtos.Requests;
using TradeSuite.Application.PreOrders.Dtos.Responses;

namespace TradeSuite.Application.PreOrders.Interfaces;

public interface IPreOrderService
{
    Task<CreatePreOrderResponseDto> CreatePreOrderAsync(CreatePreOrderRequestDto preOrderRequestDto);
}
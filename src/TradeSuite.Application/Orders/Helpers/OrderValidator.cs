using TradeSuite.Application.Orders.Dtos.Requests;
using TradeSuite.Domain.Entities;

namespace TradeSuite.Application.Orders.Helpers;

public static class OrderValidator
{
    private static bool IsItemsValid(
        PreOrder? preOrder,
        IList<CreateOrderSupplierSelectionRequestDto> createOrderSupplierSelectionsRequestDto)
    {
        return createOrderSupplierSelectionsRequestDto.Count > 0 && (preOrder?.PreOrderSuppliers?.Count ?? 0) > 0;
    }

    public static void ValidateOrderRequestDto(
        PreOrder? preOrder,
        CreateOrderFromPreOrderRequestDto dto)
    {
        IList<string> errors = [];

        if (preOrder is null)
        {   
            errors.Add("PreOrderId is invalid or not found.");
        }

        if (!IsItemsValid(preOrder, dto.CreateOrderSupplierSelections))
        {
            errors.Add("Order must contain at least one item.");
        }

        if (errors.Any())
        {
            throw new ArgumentException(string.Join(" ", errors));
        }
    }
}

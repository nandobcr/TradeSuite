using TradeSuite.Application.PreOrders.Dtos.Requests;
using TradeSuite.Domain.Entities;

namespace TradeSuite.Application.PreOrders.Helpers;

public static class PreOrderValidator
{
    public static void ValidatePreOrderRequestDto(Client? client, CreatePreOrderRequestDto createPreOrderRequestDto)
    {
        IList<string> errors = [];

        if (client is null)
        {
            errors.Add("Client invalid or not found.");
        }

        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join("; ", errors));
        }
    }
}
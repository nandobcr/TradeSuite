using TradeSuite.Domain.Entities;

namespace TradeSuite.Application.Orders.Helpers;

public static class OrderSupplierSelectionValidator
{
    public static void ValidateOrderSupplierSelection(PreOrderSupplier? preOrderSupplier)
    {
        IList<string> errors = [];

        if (preOrderSupplier is null)
        {
            errors.Add("PreOrderSupplier is invalid or not found.");
        }

        if (errors.Any())
        {
            throw new InvalidOperationException(string.Join("; ", errors));
        }
    }
}
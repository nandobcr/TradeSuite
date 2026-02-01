using TradeSuite.Application.SupplierPriceListItems.Dtos.Requests.Base;
using TradeSuite.Domain.Entities;

namespace TradeSuite.Application.SupplierPriceListItems.Helpers;

public static class SupplierPriceListItemValidator
{
    private static bool IsValidCurrency(string currency)
    {
        // Simple check for currency code format (e.g., "USD", "EUR")
        return !string.IsNullOrWhiteSpace(currency) &&
            currency.Length == SupplierPriceListItemConstants.CurrencyCodeMaxLength &&
            currency.All(char.IsUpper);
    }

    private static bool IsValidReference(string reference)
    {
        return !string.IsNullOrWhiteSpace(reference) &&
            reference.Length <= SupplierPriceListItemConstants.ReferenceMaxLength;
    }

    private static bool IsValidUnitPrice(decimal unitPrice)
    {
        return unitPrice >= 0;
    }

    public static void ValidateSupplierPriceListItemRequestDto(
        SupplierPart? supplierPart,
        BaseSupplierPriceListItemRequestDto supplierPriceListItemRequestDto)
    {
        IList<string> errors = [];

        if (supplierPart is null)
        {
            errors.Add("SupplierPartId is invalid or not found.");
        }

        if (!IsValidUnitPrice(supplierPriceListItemRequestDto.UnitPrice))
        {
            errors.Add("Unit price cannot be negative.");
        }

        if (!IsValidCurrency(supplierPriceListItemRequestDto.Currency))
        {
            errors.Add("Invalid currency format.");
        }

        if (!IsValidReference(supplierPriceListItemRequestDto.Reference))
        {
            errors.Add("Reference is required and must not exceed maximum length.");
        }

        if (errors.Any())
        {
            throw new ArgumentException(string.Join(" ", errors));
        }
    }
}
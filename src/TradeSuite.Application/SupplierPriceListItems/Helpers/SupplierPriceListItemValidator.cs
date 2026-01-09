using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.SupplierPriceListItems.Dtos.Requests.Base;
using TradeSuite.Domain.Entities;

namespace TradeSuite.Application.SupplierPriceListItems.Helpers;

public static class SupplierPriceListItemValidator
{
    private static bool IsValidCurrency(string currency)
    {
        // Simple check for currency code format (e.g., "USD", "EUR")
        return !string.IsNullOrWhiteSpace(currency) &&
            currency.Length == SupplierPriceListItemConstants.MaxCurrencyCodeLength &&
            currency.All(char.IsUpper);
    }

    public static async Task<bool> IsValidSupplierPartId(Guid supplierPartId, IRepository<SupplierPart> supplierPartRepository)
    {
        if (supplierPartId == Guid.Empty)
        {
            return false;
        }
        
        SupplierPart? supplierPart = await supplierPartRepository.GetByIdAsync(supplierPartId);

        return supplierPart != null;
    }    

    private static bool IsValidUnitPrice(decimal unitPrice)
    {
        return unitPrice >= 0;
    }

    public static async Task ValidateSupplierRequestDtoAsync(
        BaseSupplierPriceListItemRequestDto supplierPriceListItemRequestDto,
        IRepository<SupplierPart> supplierPartRepository)
    {
        IList<string> errors = [];

        if (!IsValidUnitPrice(supplierPriceListItemRequestDto.UnitPrice))
        {
            errors.Add("Unit price cannot be negative.");
        }

        if (!IsValidCurrency(supplierPriceListItemRequestDto.Currency))
        {
            errors.Add("Invalid currency format.");
        }

        if (!await IsValidSupplierPartId(supplierPriceListItemRequestDto.SupplierPartId, supplierPartRepository))
        {
            errors.Add("SupplierPartId is invalid or not found.");
        }

        if (errors.Any())
        {
            throw new ArgumentException(string.Join(" ", errors));
        }
    }
}
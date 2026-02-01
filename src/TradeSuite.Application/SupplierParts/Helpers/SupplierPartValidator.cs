using TradeSuite.Application.SupplierParts.Dtos.Requests.Base;
using TradeSuite.Domain.Entities;

namespace TradeSuite.Application.SupplierParts.Helpers;

public static class SupplierPartValidator
{
    private static bool IsValidDescription(string description)
    {
        return description.Length <= SupplierPartConstants.DescriptionMaxLength;
    }

    private static bool IsValidReference(string reference)
    {
        return !string.IsNullOrWhiteSpace(reference) && reference.Length <= SupplierPartConstants.ReferenceMaxLength;
    }

    public static void ValidateSupplierPartRequestDto(
        Supplier? supplier,
        BaseSupplierPartRequestDto supplierPartRequestDto)
    {
        IList<string> errors = [];

        if (supplier is null)
        {
            errors.Add("Supplier invalid or not found.");
        }

        if (!IsValidDescription(supplierPartRequestDto.Description))
        {
            errors.Add("Invalid description length.");
        }

        if (!IsValidReference(supplierPartRequestDto.Reference))
        {
            errors.Add("Invalid reference format.");
        }

        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join("; ", errors));
        }
    }
}
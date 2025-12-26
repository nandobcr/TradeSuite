using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.SupplierParts.Dtos.Requests.Base;
using TradeSuite.Domain.Entities;

namespace TradeSuite.Application.SupplierParts.Helpers;

public static class SupplierPartValidator
{
    public static bool IsValidDescription(string description)
    {
        return description.Length <= SupplierPartConstants.DescriptionMaxLength;
    }

    public static bool IsValidReference(string reference)
    {
        return !string.IsNullOrWhiteSpace(reference) && reference.Length <= SupplierPartConstants.ReferenceMaxLength;
    }

    public static async Task<bool> IsValidSupplierId(Guid supplierId, IRepository<Supplier> supplierRepository)
    {
        if (supplierId == Guid.Empty)
        {
            return false;
        }
        
        Supplier? supplier = await supplierRepository.GetByIdAsync(supplierId);

        return supplier != null;
    }

    public static async Task ValidateSupplierPartRequestDtoAsync(
        BaseSupplierPartRequestDto supplierPartRequestDto,
        IRepository<Supplier> supplierRepository)
    {
        IList<string> errors = [];

        if (!IsValidDescription(supplierPartRequestDto.Description))
        {
            errors.Add("Invalid description length.");
        }

        if (!IsValidReference(supplierPartRequestDto.Reference))
        {
            errors.Add("Invalid reference format.");
        }

        if (!await IsValidSupplierId(supplierPartRequestDto.SupplierId, supplierRepository))
        {
            errors.Add("Supplier invalid or not found.");
        }

        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join("; ", errors));
        }
    }
}
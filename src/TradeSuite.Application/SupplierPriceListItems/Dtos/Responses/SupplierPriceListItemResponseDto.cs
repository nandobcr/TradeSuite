namespace TradeSuite.Application.SupplierPriceListItems.Dtos.Responses;

public class SupplierPriceListItemResponseDto
{
    public Guid Id { get; set; }
    
    public required string Currency { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public Guid SupplierPartId { get; set; }

    public decimal UnitPrice { get; set; } = 0m;

    public DateTime ValidFrom { get; set; }

    public DateTime? ValidUntil { get; set; }
}
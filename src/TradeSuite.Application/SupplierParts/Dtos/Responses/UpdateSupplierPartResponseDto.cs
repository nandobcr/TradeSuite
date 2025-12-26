namespace TradeSuite.Application.SupplierParts.Dtos.Responses;

public class UpdateSupplierPartResponseDto
{
    public Guid Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public required string Reference { get; set; }

    public required Guid SupplierId { get; set; }
}
namespace TradeSuite.Application.SupplierParts.Dtos.Requests.Base;

public class BaseSupplierPartRequestDto
{
    public string Description { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;

    public Guid SupplierId { get; set; } = Guid.Empty;
}
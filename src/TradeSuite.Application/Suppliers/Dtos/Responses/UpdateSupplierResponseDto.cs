namespace TradeSuite.Application.Suppliers.Dtos.Responses;

public class UpdateSupplierResponseDto
{
    public Guid Id { get; set; }

    public string Address { get; set; } = string.Empty;

    public required string Email { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public required string Name { get; set; }

    public string Phone { get; set; } = string.Empty;
}
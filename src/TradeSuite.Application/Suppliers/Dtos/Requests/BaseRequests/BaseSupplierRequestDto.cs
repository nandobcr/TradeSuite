namespace TradeSuite.Application.Suppliers.Dtos.Requests.BaseRequests;

public class BaseSupplierRequestDto
{
    public string Address { get; set; } = string.Empty;

    public required string Email { get; set; }
    
    public required string Name { get; set; }

    public string Phone { get; set; } = string.Empty;
}
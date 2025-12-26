namespace TradeSuite.Application.Suppliers.Dtos.Requests.Base;

public class BaseSupplierRequestDto
{
    public string Address { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;
}
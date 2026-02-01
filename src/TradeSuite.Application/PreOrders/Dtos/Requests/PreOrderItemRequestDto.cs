namespace TradeSuite.Application.PreOrders.Dtos.Requests;

public class PreOrderItemRequestDto
{
    public decimal MarginPercentage { get; set; }
    
    public int Quantity { get; set; }

    public string Reference { get; set; } = string.Empty;
}

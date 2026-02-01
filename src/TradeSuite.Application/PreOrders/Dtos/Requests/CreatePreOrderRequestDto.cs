namespace TradeSuite.Application.PreOrders.Dtos.Requests;

public class CreatePreOrderRequestDto
{
    public Guid ClientId { get; set; }

    public decimal MarginPercentage { get; set; }
 
    public IList<PreOrderItemRequestDto> PreOrderItems { get; set; } = [];
}
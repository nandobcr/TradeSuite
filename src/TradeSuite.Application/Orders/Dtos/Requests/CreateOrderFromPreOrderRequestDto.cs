namespace TradeSuite.Application.Orders.Dtos.Requests;

public class CreateOrderFromPreOrderRequestDto
{
    public Guid ClientId { get; set; }

    public Guid PreOrderId { get; set; }

    public decimal MarginPercentage { get; set; } = 0m;

    public IList<CreateOrderSupplierSelectionRequestDto> CreateOrderSupplierSelections { get; set; } = [];
}
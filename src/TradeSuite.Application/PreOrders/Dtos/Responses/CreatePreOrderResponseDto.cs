namespace TradeSuite.Application.PreOrders.Dtos.Responses;

public class CreatePreOrderResponseDto
{
    public Guid ClientId { get; set; }

    public Guid PreOrderId { get; set; }

    public IList<PreOrderSupplierResponseDto> PreOrderSuppliers { get; set; } = [];
}

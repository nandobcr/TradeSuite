namespace TradeSuite.Application.Orders.Dtos.Requests;

public class CreateOrderSupplierSelectionRequestDto
{
    public Guid SupplierId { get; set; }
    
    public List<Guid> SupplierPartIds { get; set; } = [];
}
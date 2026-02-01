namespace TradeSuite.Application.PreOrders.Dtos.Responses;

public class PreOrderItemResponseDto
{
    public int Quantity { get; set; }
    
    public required string Reference { get; set; }
    
    public Guid SupplierPartId { get; set; }
    
    public decimal TotalCost { get; set; }
    
    public decimal TotalSale { get; set; }
    
    public decimal UnitCost { get; set; }
    
    public decimal UnitSale { get; set; }
}
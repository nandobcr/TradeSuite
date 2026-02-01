namespace TradeSuite.Application.PreOrders.Dtos.Responses;

public class PreOrderSupplierResponseDto
{
    public List<PreOrderItemResponseDto> PreOrderItems { get; set; } = [];

    public int Quantity { get; set; }

    public string Reference { get; set; } = string.Empty;

    public Guid SupplierId { get; set; }

    public required string SupplierName { get; set; }

    public decimal TotalCost { get; set; }

    public decimal TotalSale { get; set; }
}

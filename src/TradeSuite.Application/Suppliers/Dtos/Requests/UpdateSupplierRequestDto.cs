using TradeSuite.Application.Suppliers.Dtos.Requests.Base;

namespace TradeSuite.Application.Suppliers.Dtos.Requests;

public class UpdateSupplierRequestDto : BaseSupplierRequestDto
{
    public bool Active { get; set; } = true;
}

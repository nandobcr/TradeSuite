using TradeSuite.Application.Suppliers.Dtos.Requests.BaseRequests;

namespace TradeSuite.Application.Suppliers.Dtos.Requests;

public class UpdateSupplierRequestDto : BaseSupplierRequestDto
{
    public bool Active { get; set; } = true;

    public bool IsDeleted { get; set; } = false;
}

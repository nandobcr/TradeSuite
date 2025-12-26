using TradeSuite.Application.SupplierParts.Dtos.Requests.Base;

namespace TradeSuite.Application.SupplierParts.Dtos.Requests;

public class UpdateSupplierPartRequestDto : BaseSupplierPartRequestDto
{
    public bool IsActive { get; set; } = true;
}
using TradeSuite.Application.Clients.Dtos.Requests.Base;

namespace TradeSuite.Application.Clients.Dtos.Requests;

public class UpdateClientRequestDto : BaseClientRequestDto
{
    public bool IsActive { get; set; } = true;
}
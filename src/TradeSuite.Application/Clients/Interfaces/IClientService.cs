using TradeSuite.Application.Clients.Dtos.Requests;
using TradeSuite.Application.Clients.Dtos.Responses;

namespace TradeSuite.Application.Clients.Interfaces;

public interface IClientService
{
    Task<CreateClientResponseDto> CreateClientAsync(CreateClientRequestDto createClientRequestDto);

    Task DeleteClientAsync(Guid clientId);

    Task<IList<ClientResponseDto>> GetAllClientsAsync();

    Task<ClientResponseDto> GetClientByIdAsync(Guid clientId);

    Task<UpdateClientResponseDto> UpdateClientAsync(Guid clientId, UpdateClientRequestDto updateClientDto);
}
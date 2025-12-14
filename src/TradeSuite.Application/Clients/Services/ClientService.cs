using TradeSuite.Application.Clients.Dtos.Requests;
using TradeSuite.Application.Clients.Dtos.Responses;
using TradeSuite.Application.Clients.Helpers;
using TradeSuite.Application.Clients.Interfaces;
using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.Common.Services.Interfaces;
using TradeSuite.Domain.Common.Enums;
using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities;

namespace TradeSuite.Application.Clients.Services;

public class ClientService(
    IDateTimeProvider dateTimeProvider,
    IRepository<Client> clientRepository,
    IAuditService<Client> auditService) : IClientService
{
    public async Task<CreateClientResponseDto> CreateClientAsync(CreateClientRequestDto createClientRequestDto)
    {
        ClientValidator.ValidateClientRequestDto(createClientRequestDto);

        Client client = new(dateTimeProvider)
        {
            Address = createClientRequestDto.Address,
            Email = createClientRequestDto.Email,
            Name = createClientRequestDto.Name,
            Phone = createClientRequestDto.Phone
        };

        Guid id = await clientRepository.CreateAsync(client);

        if (id != Guid.Empty)
        {
            await auditService.LogAsync(id, client, OperationTypes.Create.ToString(), "user logado");
        }

        return new CreateClientResponseDto { Id = id };
    }

    public async Task DeleteClientAsync(Guid clientId)
    {
        _ = await clientRepository.GetByIdAsync(clientId)
            ?? throw new KeyNotFoundException($"Client with ID {clientId} not found.");

        Client? updatedClient = await clientRepository.SoftDeleteAsync(clientId);

        if (updatedClient != null && updatedClient.IsDeleted)
        {
            await auditService.LogAsync(clientId, updatedClient, OperationTypes.SoftDelete.ToString(), "user logado");
        }
    }

    public async Task<IList<ClientResponseDto>> GetAllClientsAsync()
    {
        IList<Client> clients = await clientRepository.GetAllAsync();

        return [.. clients.Select(client => new ClientResponseDto
        {
            Id = client.Id,
            Active = client.Active,
            Address = client.Address,
            Email = client.Email,
            Name = client.Name,
            Phone = client.Phone
        })];
    }

    public async Task<ClientResponseDto> GetClientByIdAsync(Guid clientId)
    {
        Client client = await clientRepository.GetByIdAsync(clientId)
            ?? throw new KeyNotFoundException($"Client with ID {clientId} not found.");
        
        return new ClientResponseDto
        {
            Id = client.Id,
            Active = client.Active,
            Address = client.Address,
            Email = client.Email,
            IsDeleted = client.IsDeleted,
            Name = client.Name,
            Phone = client.Phone
        };
    }

    public async Task<UpdateClientResponseDto> UpdateClientAsync(Guid clientId, UpdateClientRequestDto updateClientDto)
    {
        ClientValidator.ValidateClientRequestDto(updateClientDto);

        Client client = await clientRepository.GetByIdAsync(clientId)
            ?? throw new KeyNotFoundException($"Client with ID {clientId} not found.");

        client.Address = !string.IsNullOrWhiteSpace(updateClientDto.Address) ? updateClientDto.Address : client.Address;
        client.Active = updateClientDto.Active;
        client.Email = updateClientDto.Email;
        client.Name = updateClientDto.Name;
        client.Phone = !string.IsNullOrWhiteSpace(updateClientDto.Phone) ? updateClientDto.Phone : client.Phone;
        client.UpdatedAt = dateTimeProvider.UtcNow;

        bool isUpdated = await clientRepository.UpdateAsync(client);
        if (isUpdated)
        {
            await auditService.LogAsync(clientId, client, OperationTypes.Update.ToString(), "user logado");
        }

        return new UpdateClientResponseDto
        {
            Id = clientId,
            Active = client.Active,
            Address = client.Address,
            Email = client.Email,
            IsDeleted = client.IsDeleted,
            Name = client.Name,
            Phone = client.Phone
        };
    }
}
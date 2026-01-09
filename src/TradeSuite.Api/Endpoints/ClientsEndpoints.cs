using TradeSuite.Application.Clients.Dtos.Requests;
using TradeSuite.Application.Clients.Dtos.Responses;
using TradeSuite.Application.Clients.Interfaces;

namespace TradeSuite.Api.Endpoints;

public static class ClientsEndpoints
{
    public static RouteGroupBuilder MapClientsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/clients").WithTags("Clients");

        group.MapGet("/", async (IClientService clientService) =>
        {
            IList<ClientResponseDto> clients = await clientService.GetAllClientsAsync();
            return Results.Ok(clients);
        })
        .WithName("GetClients")
        .WithSummary("Get Clients")
        .WithDescription("Gets all clients.")
        .Produces<IList<ClientResponseDto>>(200)
        .ProducesProblem(500);

        group.MapGet("/{id}", async (IClientService clientService, Guid id) =>
        {
            ClientResponseDto client = await clientService.GetClientByIdAsync(id);
            return Results.Ok(client);
        })
        .WithName("GetClientById")
        .WithSummary("Get Client by ID")
        .WithDescription("Gets a client by its ID.")
        .Produces<ClientResponseDto>(200)
        .ProducesProblem(404)
        .ProducesProblem(500);

        group.MapPost("/", async (IClientService clientService, CreateClientRequestDto createClientRequestDto) =>
        {
            CreateClientResponseDto createClientResponseDto = await clientService.CreateClientAsync(createClientRequestDto);
            return Results.Ok(createClientResponseDto);
        })
        .WithName("CreateClient")
        .WithSummary("Create Client")
        .WithDescription("Creates a new client.")
        .Produces<CreateClientResponseDto>(200)
        .ProducesProblem(400)
        .ProducesProblem(500);

        group.MapPut("/{id}", async (IClientService clientService, Guid id, UpdateClientRequestDto updateClientDto) =>
        {
            UpdateClientResponseDto updateClientResponseDto = await clientService.UpdateClientAsync(id, updateClientDto);
            return Results.Ok(updateClientResponseDto);
        })
        .WithName("UpdateClient")
        .WithSummary("Update Client")
        .WithDescription("Updates an existing client.")
        .Produces<UpdateClientResponseDto>(200)
        .ProducesProblem(400)
        .ProducesProblem(404)
        .ProducesProblem(500);

        group.MapDelete("/{id}", async (IClientService clientService, Guid id) =>
        {
            await clientService.DeleteClientAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteClient")
        .WithSummary("Delete Client")
        .WithDescription("Deletes an existing client.")
        .Produces(204)
        .ProducesProblem(404)
        .ProducesProblem(500);
 
        return group;
    }
}
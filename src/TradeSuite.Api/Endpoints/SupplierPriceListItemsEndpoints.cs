using TradeSuite.Application.SupplierPriceListItems.Dtos.Requests;
using TradeSuite.Application.SupplierPriceListItems.Dtos.Responses;
using TradeSuite.Application.SupplierPriceListItems.Interfaces;

namespace TradeSuite.Api.Endpoints;

public static class SupplierPriceListItemsEndpoints
{
    public static RouteGroupBuilder MapSupplierPriceListItemsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/supplier-price-list-items").WithTags("SupplierPriceListItems");

        group.MapGet("/", async (ISupplierPriceListItemService supplierPriceListItemService) =>
        {
            IList<SupplierPriceListItemResponseDto> supplierPriceListItems = await supplierPriceListItemService.GetAllSupplierPriceListItemsAsync();
            return Results.Ok(supplierPriceListItems);
        })
        .WithName("GetSupplierPriceListItems")
        .WithSummary("Get Supplier Price List Items")
        .WithDescription("Gets all supplier price list items.")
        .Produces<IList<SupplierPriceListItemResponseDto>>(200)
        .ProducesProblem(500);

        group.MapGet("/{id}", async (ISupplierPriceListItemService supplierPriceListItemService, Guid id) =>
        {
            SupplierPriceListItemResponseDto supplierPriceListItem = await supplierPriceListItemService.GetSupplierPriceListItemByIdAsync(id);
            return Results.Ok(supplierPriceListItem);
        })
        .WithName("GetSupplierPriceListItemById")
        .WithSummary("Get Supplier Price List Item by ID")
        .WithDescription("Gets a supplier price list item by its ID.")
        .Produces<SupplierPriceListItemResponseDto>(200)
        .ProducesProblem(404)
        .ProducesProblem(500);

        group.MapPost("/", async (ISupplierPriceListItemService supplierPriceListItemService, CreateSupplierPriceListItemRequestDto createSupplierPriceListItemRequestDto) =>
        {
            CreateSupplierPriceListItemResponseDto createdSupplierPriceListItemResponseDto = await supplierPriceListItemService.CreateSupplierPriceListItemAsync(createSupplierPriceListItemRequestDto);
            return Results.Ok(createdSupplierPriceListItemResponseDto);
        })
        .WithName("CreateSupplierPriceListItem")
        .WithSummary("Create Supplier Price List Item")
        .WithDescription("Creates a new supplier price list item.")
        .Produces<CreateSupplierPriceListItemResponseDto>(200)
        .ProducesProblem(400)
        .ProducesProblem(500);

        group.MapDelete("/{id}", async (ISupplierPriceListItemService supplierPriceListItemService, Guid id) =>
        {
            await supplierPriceListItemService.DeleteSupplierPriceListItemAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteSupplierPriceListItem")
        .WithSummary("Delete Supplier Price List Item")
        .WithDescription("Deletes a supplier price list item by its ID.")
        .Produces(204)
        .ProducesProblem(404)
        .ProducesProblem(500);

        return group;
    }
}
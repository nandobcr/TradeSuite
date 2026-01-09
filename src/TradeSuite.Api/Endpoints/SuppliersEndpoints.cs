using TradeSuite.Application.Suppliers.Dtos.Requests;
using TradeSuite.Application.Suppliers.Dtos.Responses;
using TradeSuite.Application.Suppliers.Interfaces;

namespace TradeSuite.Api.Endpoints;

public static class SuppliersEndpoints
{
    public static RouteGroupBuilder MapSuppliersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/suppliers").WithTags("Suppliers");

        group.MapGet("/", async (ISupplierService supplierService) =>
        {
            IList<SupplierResponseDto> suppliers = await supplierService.GetAllSuppliersAsync();
            return Results.Ok(suppliers);
        })
        .WithName("GetSuppliers")
        .WithSummary("Get Suppliers")
        .WithDescription("Gets all suppliers.")
        .Produces<IList<SupplierResponseDto>>(200)
        .ProducesProblem(500);

        group.MapGet("/{id}", async (ISupplierService supplierService, Guid id) =>
        {
            SupplierResponseDto supplier = await supplierService.GetSupplierByIdAsync(id);
            return Results.Ok(supplier);
        })
        .WithName("GetSupplierById")
        .WithSummary("Get Supplier by ID")
        .WithDescription("Gets a supplier by its ID.")
        .Produces<SupplierResponseDto>(200)
        .ProducesProblem(404)
        .ProducesProblem(500);

        group.MapPost("/", async (ISupplierService supplierService, CreateSupplierRequestDto createSupplierRequestDto) =>
        {
            CreateSupplierResponseDto createSupplierResponseDto = await supplierService.CreateSupplierAsync(createSupplierRequestDto);
            return Results.Ok(createSupplierResponseDto);
        })
        .WithName("CreateSupplier")
        .WithSummary("Create Supplier")
        .WithDescription("Creates a new supplier.")
        .Produces<CreateSupplierResponseDto>(200)
        .ProducesProblem(400)
        .ProducesProblem(500);

        group.MapPut("/{id}", async (ISupplierService supplierService, Guid id, UpdateSupplierRequestDto updateSupplierDto) =>
        {
            UpdateSupplierResponseDto updateSupplierResponseDto = await supplierService.UpdateSupplierAsync(id, updateSupplierDto);
            return Results.Ok(updateSupplierResponseDto);
        })
        .WithName("UpdateSupplier")
        .WithSummary("Update Supplier")
        .WithDescription("Updates an existing supplier.")
        .Produces<UpdateSupplierResponseDto>(200)
        .ProducesProblem(400)
        .ProducesProblem(404)
        .ProducesProblem(500);

        group.MapDelete("/{id}", async (ISupplierService supplierService, Guid id) =>
        {
            await supplierService.DeleteSupplierAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteSupplier")
        .WithSummary("Delete Supplier")
        .WithDescription("Deletes a supplier by its ID.")
        .Produces(204)
        .ProducesProblem(404)
        .ProducesProblem(500);

        return group;
    }
}
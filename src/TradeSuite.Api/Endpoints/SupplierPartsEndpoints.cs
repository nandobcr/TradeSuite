using TradeSuite.Application.Common.Services.Interfaces;
using TradeSuite.Application.SupplierParts.Dtos.Requests;
using TradeSuite.Application.SupplierParts.Dtos.Responses;
using TradeSuite.Application.SupplierParts.Interfaces;

namespace TradeSuite.Api.Endpoints;

public static class SupplierPartsEndpoints
{
    public static RouteGroupBuilder MapSupplierPartsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/supplier-parts").WithTags("SupplierParts");

        group.MapGet("/", async (ISupplierPartService supplierPartService) =>
        {
            IList<SupplierPartResponseDto> supplierParts = await supplierPartService.GetAllSupplierPartsAsync();
            return Results.Ok(supplierParts);
        })
        .WithName("GetSupplierParts")
        .WithSummary("Get Supplier Parts")
        .WithDescription("Gets all supplier parts.")
        .Produces<IList<SupplierPartResponseDto>>(200)
        .ProducesProblem(500);

        group.MapGet("/{id}", async (ISupplierPartService supplierPartService, Guid id) =>
        {
            SupplierPartResponseDto supplierPart = await supplierPartService.GetSupplierPartByIdAsync(id);
            return Results.Ok(supplierPart);
        })
        .WithName("GetSupplierPartById")
        .WithSummary("Get Supplier Part by ID")
        .WithDescription("Gets a supplier part by its ID.")
        .Produces<SupplierPartResponseDto>(200)
        .ProducesProblem(404)
        .ProducesProblem(500);

        group.MapPost("/", async (ISupplierPartService supplierPartService, CreateSupplierPartRequestDto createSupplierPartRequestDto) =>
        {
            CreateSupplierPartResponseDto createSupplierPartResponseDto = await supplierPartService.CreateSupplierPartAsync(createSupplierPartRequestDto);
            return Results.Ok(createSupplierPartResponseDto);
        })
        .WithName("CreateSupplierPart")
        .WithSummary("Create Supplier Part")
        .WithDescription("Creates a new supplier part.")
        .Produces<CreateSupplierPartResponseDto>(200)
        .ProducesProblem(400)
        .ProducesProblem(500);

        group.MapPost("/import/{supplierId}", async (
            ISupplierPartService supplierPartService,
            IFileImportService fileImportService,
            Guid supplierId,
            HttpRequest request) =>
        {
            await fileImportService.ImportAsync(supplierId, request.Form.Files[0]);
            return Results.Ok("File imported successfully.");
        })
        .WithName("ImportSupplierParts")
        .WithSummary("Import Supplier Parts from File")
        .WithDescription("Imports supplier parts from an uploaded file.")
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<string>(200)
        .ProducesProblem(400)
        .ProducesProblem(500);

        group.MapPut("/{id}", async (ISupplierPartService supplierPartService, Guid id, UpdateSupplierPartRequestDto updateSupplierPartDto) =>
        {
            UpdateSupplierPartResponseDto updateSupplierPartResponseDto = await supplierPartService.UpdateSupplierPartAsync(id, updateSupplierPartDto);
            return Results.Ok(updateSupplierPartResponseDto);
        })
        .WithName("UpdateSupplierPart")
        .WithSummary("Update Supplier Part")
        .WithDescription("Updates an existing supplier part.")
        .Produces<UpdateSupplierPartResponseDto>(200)
        .ProducesProblem(400)
        .ProducesProblem(404)
        .ProducesProblem(500);

        group.MapDelete("/{id}", async (ISupplierPartService supplierPartService, Guid id) =>
        {
            await supplierPartService.DeleteSupplierPartAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteSupplierPart")
        .WithSummary("Delete Supplier Part")
        .WithDescription("Deletes an existing supplier part.")
        .Produces(204)
        .ProducesProblem(404)
        .ProducesProblem(500);

        return group;
    }
}
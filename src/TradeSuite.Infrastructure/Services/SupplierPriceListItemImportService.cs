using TradeSuite.Application.Common.Helpers;
using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.Common.Services.Interfaces;
using TradeSuite.Application.SupplierParts.Dtos.Requests;
using TradeSuite.Application.SupplierParts.Dtos.Responses;
using TradeSuite.Application.SupplierParts.Interfaces;
using TradeSuite.Application.SupplierPriceListItems.Dtos.Requests;
using TradeSuite.Application.SupplierPriceListItems.Interfaces;
using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities;

using ClosedXML.Excel;

using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace TradeSuite.Infrastructure.Services;

public class SupplierPriceListItemImportService(
    IDateTimeProvider dateTimeProvider,
    ISupplierPartService supplierPartService,
    IRepository<SupplierPart> supplierPartRepository,
    ISupplierPriceListItemService supplierPriceListItemService) : IFileImportService
{
    public async Task ImportAsync(Guid supplierId, IFormFile formFile)
    {
        FileImportValidator.ValidateFile(formFile);

        using var workbook = new XLWorkbook(formFile.OpenReadStream());
        IXLWorksheet worksheet = workbook.Worksheet(1);
        IEnumerable<IXLRow> rows = worksheet.RowsUsed().Skip(2); // Skip header row

        foreach (IXLRow? row in rows)
        {
            string description = row.Cell(1).GetString();
            string reference = row.Cell(2).GetString();
            string currency = row.Cell(3).GetString();
            decimal unitPrice = decimal.Parse(row.Cell(4).GetString());

            FilterDefinition<SupplierPart> filter = Builders<SupplierPart>.Filter.Eq(x => x.SupplierId, supplierId) &
                Builders<SupplierPart>.Filter.Eq(x => x.Reference, reference) &
                Builders<SupplierPart>.Filter.Eq(x => x.IsActive, true) &
                Builders<SupplierPart>.Filter.Eq(x => x.IsDeleted, false);

            SupplierPart? supplierPart = await supplierPartRepository.GetByFilterAsync(filter);
            CreateSupplierPartResponseDto? createdSupplierPartResponseDto = null;

            if (supplierPart is null)
            {
                CreateSupplierPartRequestDto createSupplierPartRequestDto = new()
                {
                    Description = description,
                    Reference = reference,
                    SupplierId = supplierId
                };

                createdSupplierPartResponseDto = await supplierPartService.CreateSupplierPartAsync(createSupplierPartRequestDto);
            }

            CreateSupplierPriceListItemRequestDto createSupplierPriceListItemRequestDto = new()
            {
                SupplierPartId = supplierPart?.Id ??
                    createdSupplierPartResponseDto?.Id ??
                    throw new ArgumentException("Failed to create or retrieve Supplier Part ID."),
                Currency = currency,
                UnitPrice = unitPrice,
                ValidFrom = dateTimeProvider.UtcNow,
            };

            await supplierPriceListItemService.CreateSupplierPriceListItemAsync(createSupplierPriceListItemRequestDto);
        }
    }
}
using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.Common.Services.Interfaces;
using TradeSuite.Application.SupplierParts.Dtos.Requests;
using TradeSuite.Application.SupplierParts.Interfaces;
using TradeSuite.Domain.Entities;

using ClosedXML.Excel;

using Microsoft.AspNetCore.Http;

using MongoDB.Driver;
using TradeSuite.Application.Common.Helpers;

namespace TradeSuite.Infrastructure.Services;

public class SupplierPartImportService(
    IRepository<SupplierPart> supplierPartRepository,
    ISupplierPartService supplierPartService) : IFileImportService
{
    private static readonly string[] _allowedExtensions = [".xls", ".xlsx", ".csv"];

    public async Task ImportAsync(Guid id, IFormFile formFile)
    {
        FileImportValidator.ValidateFile(formFile);

        using var workbook = new XLWorkbook(formFile.OpenReadStream());
        IXLWorksheet worksheet = workbook.Worksheet(1);
        IEnumerable<IXLRow> rows = worksheet.RowsUsed().Skip(2); // Skip header row

        foreach (IXLRow? row in rows)
        {
            string description = row.Cell(1).GetString();
            string reference = row.Cell(2).GetString();

            FilterDefinition<SupplierPart> filter = Builders<SupplierPart>.Filter.Eq("Reference", reference) &
                Builders<SupplierPart>.Filter.Eq("SupplierId", id);

            SupplierPart? existingSupplierPart = await supplierPartRepository.GetByFilterAsync(filter);

            if (existingSupplierPart != null)
            {
                var updateSupplierPartRequestDto = new UpdateSupplierPartRequestDto
                {
                    Description = description,
                    Reference = reference,
                    SupplierId = id
                };

                await supplierPartService.UpdateSupplierPartAsync(existingSupplierPart.Id, updateSupplierPartRequestDto);
            }
            else
            {
                var createSupplierPartRequestDto = new CreateSupplierPartRequestDto
                {
                    Description = description,
                    Reference = reference,
                    SupplierId = id
                };

                await supplierPartService.CreateSupplierPartAsync(createSupplierPartRequestDto);
            }
        }
    }
}
using TradeSuite.Application.Common.Helpers;
using TradeSuite.Application.Common.Services.Interfaces;
using TradeSuite.Application.PreOrders.Dtos.Requests;
using TradeSuite.Application.PreOrders.Interfaces;

using ClosedXML.Excel;

using Microsoft.AspNetCore.Http;

namespace TradeSuite.Infrastructure.Services;

public class PreOrderImportService(IPreOrderService preOrderService) : IFileImportService
{
    public async Task ImportAsync(Guid id, IFormFile formFile)
    {
        FileImportValidator.ValidateFile(formFile);

        using var workbook = new XLWorkbook(formFile.OpenReadStream());
        IXLWorksheet worksheet = workbook.Worksheet(1);
        IEnumerable<IXLRow> rows = worksheet.RowsUsed().Skip(2); // Skip header row

        foreach (IXLRow? row in rows)
        {
            decimal marginPercentage = row.Cell(0).GetValue<decimal>();
            string reference = row.Cell(1).GetString();
            int quantity = row.Cell(2).GetValue<int>();

            var preOrderRequestDto = new CreatePreOrderRequestDto
            {
                ClientId = id,
                MarginPercentage = marginPercentage,
                PreOrderItems =
                [
                    new PreOrderItemRequestDto
                    {
                        Reference = reference,
                        Quantity = quantity
                    }
                ]
            };

            await preOrderService.CreatePreOrderAsync(preOrderRequestDto);
        }
    }
}
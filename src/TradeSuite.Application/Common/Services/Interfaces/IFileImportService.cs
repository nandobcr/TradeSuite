using Microsoft.AspNetCore.Http;

namespace TradeSuite.Application.Common.Services.Interfaces;

public interface IFileImportService
{
    Task ImportAsync(Guid supplierId, IFormFile formFile);
}
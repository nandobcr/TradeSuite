using Microsoft.AspNetCore.Http;

namespace TradeSuite.Application.Common.Helpers;

public static class FileImportValidator
{
    private static readonly string[] _allowedExtensions = [".xls", ".xlsx", ".csv"];

    public static void ValidateFile(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            throw new FileNotFoundException("File is empty or not provided.");
        }

        string extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();

        if (!_allowedExtensions.Contains(extension))
        {
            throw new ArgumentException("Invalid format. Use XLS, XLSX, or CSV.");
        }
    }
}
using TradeSuite.Application.Suppliers.Dtos.Requests.BaseRequests;

using System.Net.Mail;
using System.Text.RegularExpressions;

namespace TradeSuite.Application.Suppliers.Helpers;

public static class SupplierValidator
{
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        
        return name.Length <= SupplierConstants.NameMaxLength;
    }

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (phoneNumber.Length > SupplierConstants.PhoneMaxLength)
        {
            return false;
        }

        string pattern = @"^\+(\d{1,3})\s?\d{4,14}(?:x\d+)?$";

        return Regex.IsMatch(phoneNumber, pattern);
    }

    private static bool IsValidAddress(string address)
    {
        return address.Length <= SupplierConstants.AddressMaxLength;
    }

    public static void ValidateSupplierRequestDto(BaseSupplierRequestDto supplierRequestDto)
    {
        IList<string> errors = [];

        if (!IsValidEmail(supplierRequestDto.Email))
        {
            errors.Add("Invalid email format.");
        }

        if (!IsValidName(supplierRequestDto.Name))
        {
            errors.Add($"Name is required and must not exceed {SupplierConstants.NameMaxLength} characters.");
        }
        
        if (!IsValidPhoneNumber(supplierRequestDto.Phone))
        {
            errors.Add("Invalid phone number format.");
        }
        
        if (!IsValidAddress(supplierRequestDto.Address))
        {
            errors.Add($"Address must not exceed {SupplierConstants.AddressMaxLength} characters.");
        }

        if (errors.Any())
        {
            throw new ArgumentException(string.Join(" ", errors));
        }
    }
}
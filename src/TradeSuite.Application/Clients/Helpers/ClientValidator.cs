using TradeSuite.Application.Clients.Dtos.Requests.Base;

using System.Net.Mail;
using System.Text.RegularExpressions;

namespace TradeSuite.Application.Clients.Helpers;

public static class ClientValidator
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
        
        return name.Length <= ClientConstants.NameMaxLength;
    }

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (phoneNumber.Length > ClientConstants.PhoneMaxLength)
        {
            return false;
        }

        string pattern = @"^\+(\d{1,3})\s?\d{4,14}(?:x\d+)?$";

        return Regex.IsMatch(phoneNumber, pattern);
    }

    private static bool IsValidAddress(string address)
    {
        return address.Length <= ClientConstants.AddressMaxLength;
    }

    public static void ValidateClientRequestDto(BaseClientRequestDto clientRequestDto)
    {
        IList<string> errors = [];

        if (!IsValidEmail(clientRequestDto.Email))
        {
            errors.Add("Invalid email format.");
        }

        if (!IsValidName(clientRequestDto.Name))
        {
            errors.Add($"Name is required and must not exceed {ClientConstants.NameMaxLength} characters.");
        }
        
        if (!IsValidPhoneNumber(clientRequestDto.Phone))
        {
            errors.Add("Invalid phone number format.");
        }
        
        if (!IsValidAddress(clientRequestDto.Address))
        {
            errors.Add($"Address must not exceed {ClientConstants.AddressMaxLength} characters.");
        }

        if (errors.Any())
        {
            throw new ArgumentException(string.Join(" ", errors));
        }
    }
}
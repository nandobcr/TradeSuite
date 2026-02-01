using TradeSuite.Domain.Entities.Base;

namespace TradeSuite.Domain.Entities;

public class User(DateTime utcNow) : BaseEntity(utcNow)
{
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public IList<string> SupplierIds { get; set; } = [];

    public string Username { get; set; } = string.Empty;
}
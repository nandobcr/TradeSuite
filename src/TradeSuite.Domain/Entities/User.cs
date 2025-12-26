using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities.Base;

namespace TradeSuite.Domain.Entities;

public class User(IDateTimeProvider dateTimeProvider) : BaseEntity(dateTimeProvider)
{
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public IList<string> SupplierIds { get; set; } = [];

    public string Username { get; set; } = string.Empty;
}
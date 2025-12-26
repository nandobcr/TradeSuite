using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities.Base;

namespace TradeSuite.Domain.Entities;

public class Client(IDateTimeProvider dateTimeProvider) : BaseEntity(dateTimeProvider)
{
    public string Address { get; set; } = string.Empty;

    public required string Email { get; set; }
    
    public required string Name { get; set; }

    public string Phone { get; set; } = string.Empty;
}
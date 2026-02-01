using TradeSuite.Domain.Entities.Base;

namespace TradeSuite.Domain.Entities;

public class Client(DateTime utcNow) : BaseEntity(utcNow)
{
    public string Address { get; set; } = string.Empty;

    public required string Email { get; set; }
    
    public required string Name { get; set; }

    public string Phone { get; set; } = string.Empty;
}
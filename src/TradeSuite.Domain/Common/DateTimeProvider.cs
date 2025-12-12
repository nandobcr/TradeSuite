using TradeSuite.Domain.Common.Interfaces;

namespace TradeSuite.Domain.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
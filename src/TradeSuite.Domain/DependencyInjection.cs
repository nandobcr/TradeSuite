using TradeSuite.Domain.Common;
using TradeSuite.Domain.Common.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace TradeSuite.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        return services;
    }
}
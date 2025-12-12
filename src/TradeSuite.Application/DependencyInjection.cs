using TradeSuite.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace TradeSuite.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddDomainServices();
        
        return services;
    }
}
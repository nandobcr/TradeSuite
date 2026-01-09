using TradeSuite.Application.Clients.Interfaces;
using TradeSuite.Application.Clients.Services;
using TradeSuite.Application.Common.Repositories.Interfaces;
using TradeSuite.Application.Common.Services.Interfaces;
using TradeSuite.Application.Suppliers.Interfaces;
using TradeSuite.Application.Suppliers.Services;
using TradeSuite.Application.SupplierParts.Interfaces;
using TradeSuite.Application.SupplierParts.Services;
using TradeSuite.Application.SupplierPriceListItems.Interfaces;
using TradeSuite.Application.SupplierPriceListItems.Services;
using TradeSuite.Infrastructure.Configs;
using TradeSuite.Infrastructure.Database;
using TradeSuite.Infrastructure.Repositories;
using TradeSuite.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using MongoDB.Driver;

namespace TradeSuite.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoClient>(sp =>
        {
            MongoDbSettings settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        services.AddSingleton<MongoDbContext>();
        
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<ISupplierPartService, SupplierPartService>();
        services.AddScoped<ISupplierPriceListItemService, SupplierPriceListItemService>();
        services.AddScoped<IFileImportService, SupplierPartImportService>();
        services.AddScoped<IFileImportService, SupplierPriceListItemImportService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IAuditRepository<>), typeof(AuditRepository<>));
        services.AddScoped(typeof(IAuditService<>), typeof(AuditService<>));

        return services;
    }
}
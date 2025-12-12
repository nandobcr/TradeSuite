using TradeSuite.Domain.Entities;
using TradeSuite.Infrastructure.Configs;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

namespace TradeSuite.Infrastructure.Database;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public IMongoCollection<Order> Orders 
        => _database.GetCollection<Order>("Orders");

    public IMongoCollection<Client> Clients 
        => _database.GetCollection<Client>("Clients");

    public IMongoCollection<Supplier> Suppliers 
        => _database.GetCollection<Supplier>("Suppliers");

    public IMongoCollection<SupplierPart> SupplierParts 
        => _database.GetCollection<SupplierPart>("SupplierParts");

    public IMongoCollection<SupplierPriceListItem> SupplierPriceList 
        => _database.GetCollection<SupplierPriceListItem>("SupplierPriceList");

    
    public MongoDbContext(IMongoClient client, IOptions<MongoDbSettings> settings)
    {
        _database = client.GetDatabase(settings.Value.DatabaseName);
        EnsureIndexes();
    }

    private void EnsureIndexes()
    {
        CreateClientIndexes();
        CreateOrderIndexes();
        CreatePriceListIndexes();
        CreateSupplierIndexes();
        CreateSupplierPartIndexes();
    }

    private void CreateClientIndexes()
    {
        Clients.Indexes.CreateOne(
            new CreateIndexModel<Client>(
                Builders<Client>.IndexKeys
                    .Ascending(c => c.Email),
                new CreateIndexOptions { Unique = true })
        );
    }

    private void CreateOrderIndexes()
    {
        Orders.Indexes.CreateOne(
            new CreateIndexModel<Order>(Builders<Order>.IndexKeys
                .Ascending(x => x.ClientId)));

        // Optional: find orders by supplier (via items)
        Orders.Indexes.CreateOne(
            new CreateIndexModel<Order>(Builders<Order>.IndexKeys
                .Ascending("Items.SupplierId")));
    }    

    private void CreatePriceListIndexes()
    {
        // PriceListItem: SupplierPartId + ValidFrom
        SupplierPriceList.Indexes.CreateOne(
            new CreateIndexModel<SupplierPriceListItem>(
                Builders<SupplierPriceListItem>.IndexKeys
                    .Ascending(x => x.SupplierPartId)
                    .Ascending(x => x.ValidFrom)
            )
        );

        // Optional: useful for queries by validity interval
        SupplierPriceList.Indexes.CreateOne(
            new CreateIndexModel<SupplierPriceListItem>(
                Builders<SupplierPriceListItem>.IndexKeys
                    .Ascending(x => x.ValidUntil))
        );
    }

    private void CreateSupplierIndexes()
    {
        Suppliers.Indexes.CreateOne(
            new CreateIndexModel<Supplier>(
                Builders<Supplier>.IndexKeys
                    .Ascending(s => s.Name),
                new CreateIndexOptions { Unique = true })
        );
    }        

    private void CreateSupplierPartIndexes()
    {
        // SupplierPart: Supplier + Reference must be unique per supplier
        SupplierParts.Indexes.CreateOne(
            new CreateIndexModel<SupplierPart>(
                Builders<SupplierPart>.IndexKeys
                    .Ascending(x => x.SupplierId)
                    .Ascending(x => x.Reference),
                new CreateIndexOptions { Unique = true })
        );
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}

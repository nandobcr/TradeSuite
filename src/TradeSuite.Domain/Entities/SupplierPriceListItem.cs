using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities.Base;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities;

public class SupplierPriceListItem(IDateTimeProvider dateTimeProvider) : BaseEntity(dateTimeProvider)
{
    public string Currency { get; set; } = "EUR";

    [BsonRepresentation(BsonType.String)]
    public required Guid SupplierPartId { get; set; }

    public decimal UnitPrice { get; set; } = 0m;

    public DateTime ValidFrom { get; set; } = dateTimeProvider.UtcNow;

    public DateTime? ValidUntil { get; set; } = null;
}
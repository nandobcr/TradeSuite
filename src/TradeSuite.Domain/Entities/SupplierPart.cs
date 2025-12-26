using TradeSuite.Domain.Common.Interfaces;
using TradeSuite.Domain.Entities.Base;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities;

public class SupplierPart(IDateTimeProvider dateTimeProvider) : BaseEntity(dateTimeProvider)
{
    public string Description { get; set; } = string.Empty;

    public required string Reference { get; set; }

    [BsonRepresentation(BsonType.String)]
    public required Guid SupplierId { get; set; }
}
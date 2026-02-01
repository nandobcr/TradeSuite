using TradeSuite.Domain.Common.Enums;
using TradeSuite.Domain.Entities.Base;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TradeSuite.Domain.Entities;

public class PreOrder(DateTime utcNow) : BaseEntity(utcNow) 
{
    [BsonRepresentation(BsonType.String)]
    public Guid ClientId { get; set; }

    public List<PreOrderSupplier> PreOrderSuppliers { get; set; } = [];

    public string Status { get; set; } = PreOrderStatuses.Draft.ToString();

    public decimal TotalCost { get; set; }

}
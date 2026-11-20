using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace StockScribe.Api.Infrastructure.Mongo;

/// <summary>One business action, append-only (ADR-0001: audit lives in MongoDB).</summary>
public sealed class AuditEntry
{
    [BsonId] public ObjectId Id { get; init; }
    public DateTime At { get; init; }
    /// <summary>e.g. "PurchaseOrder.Approved".</summary>
    public string Action { get; init; } = null!;
    public string EntityType { get; init; } = null!;
    [BsonRepresentation(BsonType.String)] public Guid EntityId { get; init; }
    public string? EntityNumber { get; init; }
    [BsonRepresentation(BsonType.String)] public Guid UserId { get; init; }
    public string UserName { get; init; } = null!;
    public string UserRole { get; init; } = null!;
    public BsonDocument? Data { get; init; }
}

public sealed class AuditLog(IMongoDatabase database)
{
    public const string CollectionName = "audit_log";

    public IMongoCollection<AuditEntry> Collection { get; } = database.GetCollection<AuditEntry>(CollectionName);

    public Task WriteAsync(AuditEntry entry, CancellationToken ct = default) =>
        Collection.InsertOneAsync(entry, cancellationToken: ct);
}

using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace StockScribe.Api.Infrastructure.Mongo;

/// <summary>
/// Readiness check for MongoDB: runs the <c>ping</c> command against the app database.
/// Written in-house instead of AspNetCore.HealthChecks.MongoDb to keep dependencies small.
/// </summary>
public sealed class MongoHealthCheck(IMongoDatabase database) : IHealthCheck
{
    private static readonly BsonDocument Ping = new("ping", 1);

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await database.RunCommandAsync<BsonDocument>(Ping, cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, "MongoDB ping failed", ex);
        }
    }
}

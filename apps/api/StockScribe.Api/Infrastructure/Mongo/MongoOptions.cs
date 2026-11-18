using System.ComponentModel.DataAnnotations;

namespace StockScribe.Api.Infrastructure.Mongo;

/// <summary>Bound from the "Mongo" section, i.e. env vars Mongo__ConnectionString and Mongo__Database.</summary>
public sealed class MongoOptions
{
    public const string SectionName = "Mongo";

    [Required]
    public string ConnectionString { get; init; } = string.Empty;

    [Required]
    public string Database { get; init; } = string.Empty;
}

namespace StockScribe.Api.Domain;

/// <summary>
/// A business rule said no. Endpoints turn it into RFC 9457 ProblemDetails with
/// <see cref="Type"/> as the problem type and <see cref="Title"/> as Thai text the screen can show.
/// </summary>
public sealed class DomainException(string type, string title, int statusCode) : Exception(title)
{
    public string Type { get; } = type;
    public string Title { get; } = title;
    public int StatusCode { get; } = statusCode;

    public static DomainException InvalidTransition(string title) => new("invalid-state-transition", title, 409);
    public static DomainException Forbidden(string type, string title) => new(type, title, 403);
    public static DomainException Invalid(string type, string title) => new(type, title, 422);
}

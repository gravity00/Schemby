namespace Schemby.Specifications;

/// <summary>
/// Aggregates a database specification.
/// </summary>
/// <param name="Version">The specification version</param>
/// <param name="CreatedAt">The date and time when the specification was created</param>
/// <param name="Database">The database specification was created</param>
public record Specification(
    int Version,
    DateTimeOffset CreatedAt,
    Database Database
)
{
    /// <summary>
    /// Specification author.
    /// </summary>
    public string? Author { get; init; }

    /// <summary>
    /// Specification description.
    /// </summary>
    public string? Description { get; init; }
}
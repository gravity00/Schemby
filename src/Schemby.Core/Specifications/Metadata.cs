namespace Schemby.Specifications;

/// <summary>
/// Represents a collection of database specifications.
/// </summary>
/// <param name="Version">The specification version</param>
/// <param name="CreatedAt">The date and time when the specification was created</param>
public record Metadata(
    int Version,
    DateTimeOffset CreatedAt
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
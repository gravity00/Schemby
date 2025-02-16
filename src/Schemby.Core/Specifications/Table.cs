namespace Schemby.Specifications;

/// <summary>
/// Represents a table specification.
/// </summary>
/// <param name="Name">The table name</param>
/// <param name="Columns">Collection of column specifications</param>
public record Table(
    string Name,
    IEnumerable<Column> Columns
)
{
    /// <summary>
    /// Specification description.
    /// </summary>
    public string? Description { get; init; }
}
namespace Schemby.Specifications;

/// <summary>
/// Represents a table specification.
/// </summary>
/// <param name="Name">The table name</param>
/// <param name="Columns">Collection of column specifications</param>
public record Table(
    string Name,
    IReadOnlyCollection<Column> Columns
)
{
    /// <summary>
    /// Table primary key specification.
    /// </summary>
    public PrimaryKey? PrimaryKey { get; init; }

    /// <summary>
    /// Collection of foreign keys specifications.
    /// </summary>
    public IReadOnlyCollection<ForeignKey> ForeignKeys { get; init; } = [];

    /// <summary>
    /// Collection of indexes specifications.
    /// </summary>
    public IReadOnlyCollection<Index> Indexes { get; init; } = [];

    /// <summary>
    /// Table description.
    /// </summary>
    public string? Description { get; init; }
}
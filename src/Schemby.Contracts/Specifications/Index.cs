namespace Schemby.Specifications;

/// <summary>
/// Represents a table index specification.
/// </summary>
/// <param name="Name">The index name</param>
/// <param name="Unique">Is the index unique?</param>
/// <param name="Columns">The index columns</param>
public record Index(
    string Name,
    bool Unique,
    IReadOnlyCollection<IndexColumn> Columns
);
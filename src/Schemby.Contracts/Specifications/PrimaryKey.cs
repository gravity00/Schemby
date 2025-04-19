namespace Schemby.Specifications;

/// <summary>
/// Represents a table primary key specification.
/// </summary>
/// <param name="Name">The primary key name</param>
/// <param name="Columns">Collection of column names</param>
public record PrimaryKey(
    string Name,
    IReadOnlyCollection<string> Columns
);
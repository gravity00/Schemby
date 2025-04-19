namespace Schemby.Specifications;

/// <summary>
/// Represents a database specification.
/// </summary>
/// <param name="Metadata">The database specification metadata</param>
/// <param name="Name">The database name</param>
/// <param name="Tables">Collection of table specifications</param>
public record Database(
    Metadata Metadata,
    string Name,
    IReadOnlyCollection<Table> Tables
);
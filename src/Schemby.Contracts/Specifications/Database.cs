namespace Schemby.Specifications;

/// <summary>
/// Represents a database specification.
/// </summary>
/// <param name="Name">The database name</param>
/// <param name="Tables">Collection of table specifications</param>
public record Database(
    string Name,
    IReadOnlyDictionary<string, Table> Tables
);
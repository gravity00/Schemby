namespace Schemby.Specifications;

/// <summary>
/// Represents a database specification.
/// </summary>
/// <param name="Name">The database name</param>
/// <param name="Tables">Collection of table specifications</param>
/// <param name="Description">Optional specification description</param>
public record Database(
    string Name,
    IEnumerable<Table> Tables,
    string? Description = null
);
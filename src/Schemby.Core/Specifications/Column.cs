namespace Schemby.Specifications;

/// <summary>
/// Represents a column specification.
/// </summary>
/// <param name="Name">The column name</param>
/// <param name="Type">The column type</param>
/// <param name="Description">Optional specification description</param>
public record Column(
    string Name,
    ColumnType Type,
    string? Description = null
);
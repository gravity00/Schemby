namespace Schemby.Specifications;

/// <summary>
/// Represents a table index column specification.
/// </summary>
/// <param name="Name">The column name</param>
/// <param name="Ascending">Is the column in ascending order?</param>
public record IndexColumn(
    string Name,
    bool Ascending
);
namespace Schemby.Specifications;

/// <summary>
/// Represents a table foreign key specification.
/// </summary>
/// <param name="Name">The foreign key name</param>
/// <param name="Table">The foreign key table name</param>
/// <param name="ColumnMap">Collection of column mapping between tables</param>
public record ForeignKey(
    string Name,
    string Table,
    IReadOnlyDictionary<string, string> ColumnMap
);
namespace Schemby.Entities;

internal record TableIndexViewEntity
{
    public string DatabaseName { get; init; } = string.Empty;

    public string TableName { get; init; } = string.Empty;

    public string IndexName { get; init; } = string.Empty;

    public string ColumnName { get; init; } = string.Empty;

    public bool IsUnique { get; init; }

    public bool IsAscending { get; init; }
}
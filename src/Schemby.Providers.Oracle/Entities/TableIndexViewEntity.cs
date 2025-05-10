namespace Schemby.Providers.Entities;

internal record TableIndexViewEntity
{
    public string TableName { get; init; } = string.Empty;

    public string IndexName { get; init; } = string.Empty;

    public string ColumnName { get; init; } = string.Empty;

    public bool IsUnique { get; init; }

    public bool IsAscending { get; init; }
}
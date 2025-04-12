namespace Schemby.Entities;

internal record TablePrimaryKeyViewEntity
{
    public string TableName { get; init; } = string.Empty;

    public string ConstraintName { get; init; } = string.Empty;

    public string ColumnName { get; init; } = string.Empty;
}
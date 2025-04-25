namespace Schemby.Entities;

internal record TableConstraintViewEntity
{
    public string Name { get; init; } = string.Empty;

    public TableConstraintTypeEntity Type { get; init; }

    public string TableName { get; init; } = string.Empty;

    public string ColumnName { get; init; } = string.Empty;

    public string? ConstraintNameFk { get; init; }
}
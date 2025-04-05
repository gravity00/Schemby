namespace Schemby.Entities;

internal record TableColumnViewEntity
{
    public string DatabaseName { get; init; } = string.Empty;

    public string TableName { get; init; } = string.Empty;
    
    public string ColumnName { get; init; } = string.Empty;
    
    public string Type { get; init; } = string.Empty;
    
    public bool IsNullable { get; init; }
    
    public int Length { get; init; }
    
    public int? Precision { get; init; }
    
    public int? Scale { get; init; }
    
    public string? DefaultValue { get; init; }
    
    public string? Description { get; init; }
}
namespace Schemby.Entities;

internal record TableColumnViewEntity
{
    public string DatabaseName { get; set; } = string.Empty;

    public string TableName { get; set; } = string.Empty;
    
    public string ColumnName { get; set; } = string.Empty;
    
    public string Type { get; set; } = string.Empty;
    
    public bool Nullable { get; set; }
    
    public int Length { get; set; }
    
    public int? Precision { get; set; }
    
    public int? Scale { get; set; }
    
    public string? DefaultValue { get; set; }
    
    public string? Description { get; set; }
}
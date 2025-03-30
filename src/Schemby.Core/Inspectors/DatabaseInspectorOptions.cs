namespace Schemby.Inspectors;

/// <summary>
/// Database inspector options.
/// </summary>
public record struct DatabaseInspectorOptions
{
    /// <summary>
    /// Optional table filter expression.
    /// </summary>
    public string? TableFilter { get; init; }

    /// <summary>
    /// Optional column filter expression.
    /// </summary>
    public string? ColumnFilter { get; init; }
}
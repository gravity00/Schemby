namespace Schemby;

/// <summary>
/// Database inspector options.
/// </summary>
public record struct InspectOptions
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
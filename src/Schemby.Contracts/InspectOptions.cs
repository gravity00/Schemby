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
    /// Is the table filter exclusive?
    /// </summary>
    public bool IsExclusiveTableFilter { get; init; }

    /// <summary>
    /// Optional column filter expression.
    /// </summary>
    public string? ColumnFilter { get; init; }

    /// <summary>
    /// Is the column filter exclusive?
    /// </summary>
    public bool IsExclusiveColumnFilter { get; init; }
}
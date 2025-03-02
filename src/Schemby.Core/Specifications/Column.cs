namespace Schemby.Specifications;

/// <summary>
/// Represents a column specification.
/// </summary>
/// <param name="Name">The column name</param>
/// <param name="Type">The column type</param>
/// <param name="TypeNative">The column native type</param>
/// <param name="Nullable">Is the column nullable?</param>
/// <param name="Length">The column length</param>
public record Column(
    string Name,
    ColumnType Type,
    string TypeNative,
    bool Nullable,
    int Length
)
{
    /// <summary>
    /// Column description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The column precision.
    /// </summary>
    public int? Precision { get; init; }

    /// <summary>
    /// The column scale.
    /// </summary>
    public int? Scale { get; init; }

    /// <summary>
    /// The column default value.
    /// </summary>
    public string? DefaultValue { get; init; }
}
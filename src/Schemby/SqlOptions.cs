namespace Schemby;

/// <summary>
/// Options for executing a SQL command.
/// </summary>
public record struct SqlOptions
{
    /// <summary>
    /// The SQL command parameters.
    /// </summary>
    public object? Parameters { get; init; }

    /// <summary>
    /// The command timeout in seconds.
    /// </summary>
    public int? Timeout { get; init; }
}
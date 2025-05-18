namespace Schemby.Commands;

public record InspectCommand : Command
{
    public string ConnectionString { get; init; } = string.Empty;

    public string Provider { get; init; } = string.Empty;

    public string Database { get; init; } = string.Empty;

    public string Output { get; init; } = string.Empty;

    public string? TableFilter { get; init; }

    public bool IsExclusiveTableFilter { get; init; }

    public string? ColumnFilter { get; init; }

    public bool IsExclusiveColumnFilter { get; init; }

    public InspectOutputFormat Format { get; init; }

    public string? Author { get; init; }

    public string? Description { get; init; }
}
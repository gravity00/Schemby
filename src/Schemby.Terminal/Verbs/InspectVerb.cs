using CommandLine;

namespace Schemby.Verbs;

[Verb("inspect", HelpText = "Inspect a database and output a file specification.")]
public record InspectVerb : Verb
{
    [Value(0, Required = true, HelpText = "The connection string.")]
    public string ConnectionString { get; init; } = string.Empty;

    [Option('p', "provider", Required = true, HelpText = "The database provider.")]
    public string Provider { get; init; } = string.Empty;

    [Option('d', "database", Required = true, HelpText = "The database name.")]
    public string Database { get; init; } = string.Empty;

    [Option('t', "table", Required = false, HelpText = "The table filter expression.")]
    public string? TableFilter { get; init; }

    [Option('c', "column", Required = false, HelpText = "The column filter expression.")]
    public string? ColumnFilter { get; init; }
}
using CommandLine;
using CommandLine.Text;

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

    [Option('o', "output", Required = true, HelpText = "The output file path.")]
    public string Output { get; init; } = string.Empty;

    [Option('t', "table", HelpText = "The table filter expression.")]
    public string? TableFilter { get; init; }

    [Option("exclusive-table", HelpText = "Is the table filter exclusive?")]
    public bool IsExclusiveTableFilter { get; init; }

    [Option('c', "column", HelpText = "The column filter expression.")]
    public string? ColumnFilter { get; init; }

    [Option("exclusive-column", HelpText = "Is the column filter exclusive?")]
    public bool IsExclusiveColumnFilter { get; init; }

    [Option('f', "format", Default = InspectVerbOutputFormat.Yaml, HelpText = "The output format (yaml, json, xml).")]
    public InspectVerbOutputFormat Format { get; init; }

    [Option("author", HelpText = "The author of the specification.")]
    public string? Author { get; init; }

    [Option("description", HelpText = "The description of the specification.")]
    public string? Description { get; init; }

    [Usage(ApplicationAlias = "schemby")]
    public static IEnumerable<Example> Examples { get; } =
    [
        new Example("Simple inspection output to a YAML file", new InspectVerb
        {
            ConnectionString = "DATA SOURCE=localhost;USER ID=the_user;PASSWORD=the_password;",
            Provider = "oracle",
            Database = "the_database",
            Output = "database.yaml",
        }),
        new Example("Fully configured inspection output to a JSON file", new InspectVerb
        {
            ConnectionString = "Server=localhost;User Id=the_user;Password=the_password;",
            Provider = "sqlServer",
            Database = "the_database",
            Output = "database.yaml",
            TableFilter = "dbo%",
            ColumnFilter = "dbo%.%",
            Format = InspectVerbOutputFormat.Json,
            Author = "John Doe",
            Description = "Specification for John Doe database.",
            Verbose = true
        })
    ];
}
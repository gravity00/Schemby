using CommandLine;

namespace Schemby.Verbs;

public abstract record Verb
{
    [Option("verbose", Default = false, HelpText = "Enable verbose output.")]
    public bool Verbose { get; init; }
}
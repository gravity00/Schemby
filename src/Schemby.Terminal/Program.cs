using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Schemby.Commands;
using Schemby.Providers;
using Schemby.Verbs;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => cts.Cancel();
var ct = cts.Token;

var builder = Host.CreateApplicationBuilder(args);

ConfigureLogging(
    builder.Logging,
    args
);
ConfigureServices(
    builder.Services
);

using var host = builder.Build();
var logger = host.Services.GetRequiredService<ILogger<Program>>();
var mediator = host.Services.GetRequiredService<IMediator>();
var parser = host.Services.GetRequiredService<Parser>();

return parser.ParseArguments<
    InspectVerb,
    object
>(args).MapResult(
    (InspectVerb verb) => RunInspectVerb(logger, mediator, verb, ct),
    _ => 1
);

static void ConfigureLogging(
    ILoggingBuilder logging,
    string[] args
)
{
    var verbose = args.Any(arg => "--verbose".Equals(arg, StringComparison.OrdinalIgnoreCase));
    logging.SetMinimumLevel(verbose ? LogLevel.Trace : LogLevel.Information);
}

static void ConfigureServices(
    IServiceCollection services
)
{
    services.AddSchemby(providers =>
    {
        providers["oracle"] = new OracleProviderInstaller();
    });
    services.AddMediator(options =>
    {
        options.AddPipelineForLogging(o =>
        {
            o.Level = LogLevel.Debug;
            o.LogCommandResult = true;
        });
        options.AddPipelineForValidation(o =>
        {
            o.ValidateCommand = true;
        });
        options.AddHandlersFromAssemblyOf<Program>();
        options.AddValidatorsFromAssemblyOf<Program>();
    });
    services.AddSingleton(_ => new Parser(settings =>
    {
        settings.CaseInsensitiveEnumValues = true;
        settings.HelpWriter = Console.Error;
    }));
}

static int RunInspectVerb(ILogger<Program> logger, IMediator mediator, InspectVerb verb, CancellationToken ct)
{
    return RunCommand(logger, mediator, new InspectCommand
    {
        ConnectionString = verb.ConnectionString,
        Provider = verb.Provider,
        Database = verb.Database,
        Output = verb.Output,
        TableFilter = verb.TableFilter,
        ColumnFilter = verb.ColumnFilter,
        Format = (InspectOutputFormat)verb.Format,

        Verbose = verb.Verbose,
    }, ct);
}

static int RunCommand<TCommand>(ILogger<Program>  logger, IMediator mediator, TCommand command, CancellationToken ct)
    where TCommand : Schemby.Commands.Command
{
    logger.LogInformation("Command execution started [Command:{CommandType}]", typeof(TCommand).Name);
    try
    {
        mediator.SendAsync(command, ct)
            .ConfigureAwait(false).GetAwaiter().GetResult();
    }
    catch (Exception e)
    {
        logger.LogCritical(e, "Command execution failed.");
        return 1;
    }

    logger.LogInformation("Command executed successfully.");
    return 0;
}
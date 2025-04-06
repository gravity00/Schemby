using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Schemby;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => cts.Cancel();
var ct = cts.Token;

var builder = Host.CreateApplicationBuilder(args);

ConfigureServices(
    builder.Services
);

using var host = builder.Build();

var inspectorFactory = host.Services.GetRequiredService<IInspectorFactory>();

var inspector = inspectorFactory.Create("oracle");

return;

static void ConfigureServices(
    IServiceCollection services
)
{
    services.AddSingleton<ISqlRunner, SqlRunner>();
    services.AddSingleton<IInspectorFactory, InspectorFactory>();
    services.AddKeyedSingleton<IInspector, OracleInspector>("oracle");

}
namespace Schemby.Commands.Handlers;

public class InspectCommandHandler(
    ILogger<InspectCommandHandler> logger,
    IInspectorFactory inspectorFactory
) : ICommandHandler<InspectCommand>
{
    public async Task HandleAsync(InspectCommand cmd, CancellationToken ct)
    {
        var inspector = inspectorFactory.Create(cmd.Provider);

        var database = await inspector.InspectAsync(cmd.ConnectionString, cmd.Database, new InspectOptions
        {
            TableFilter = cmd.TableFilter,
            ColumnFilter = cmd.ColumnFilter,
        }, ct);
    }
}
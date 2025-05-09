using Schemby.Specifications;

namespace Schemby.Commands.Handlers;

public class InspectCommandHandler(
    ILogger<InspectCommandHandler> logger,
    IInspectorFactory inspectorFactory,
    ISerializerFactory serializerFactory
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

        var specification = new Specification(
            1,
            DateTimeOffset.UtcNow,
            database
        )
        {
            Author = cmd.Author,
            Description = cmd.Description,
        };

#if !NET48
        await
#endif
            using var outputStream = CreateOutputFile(cmd.Output);

        var serializer = serializerFactory.Create(cmd.Format.ToString("G"));

        await serializer.SerializeToStreamAsync(specification, outputStream, ct);
    }

    private static Stream CreateOutputFile(string outputPath)
    {
        var filePath = PathEx.GetFullPath(outputPath, Directory.GetCurrentDirectory());
        
        var fileInfo = new FileInfo(filePath);
        if (fileInfo.Exists)
            fileInfo.Delete();
        else
            fileInfo.Directory?.Create();

        return new FileStream(
            fileInfo.FullName,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            1024,
            FileOptions.Asynchronous
        );
    }
}
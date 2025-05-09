using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Schemby;

/// <summary>
/// Factory for creating specification serializers.
/// </summary>
/// <param name="logger">The logger instance</param>
/// <param name="serviceProvider">The dependency injection service provider</param>
public class SerializerFactory(
    ILogger<SerializerFactory> logger,
    IServiceProvider serviceProvider
) : ISerializerFactory
{
    /// <inheritdoc />
    public ISerializer Create(string format)
    {
        if (format is null) throw new ArgumentNullException(nameof(format));

        format = format.ToLowerInvariant();

        logger.LogDebug("Creating specification serializer for '{Format}'", format);

        return serviceProvider.GetRequiredKeyedService<ISerializer>(format);
    }
}
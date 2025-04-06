using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Schemby;

/// <summary>
/// Factory for creating database inspectors.
/// </summary>
/// <param name="logger">The logger instance</param>
/// <param name="serviceProvider">The dependency injection service provider</param>
public class InspectorFactory(
    ILogger<InspectorFactory> logger,
    IServiceProvider serviceProvider
) : IInspectorFactory
{
    /// <inheritdoc />
    public IInspector Create(string provider)
    {
        if (provider is null) throw new ArgumentNullException(nameof(provider));

        provider = provider.ToLowerInvariant();

        logger.LogDebug("Creating database inspector for '{Provider}'", provider);

        return serviceProvider.GetRequiredKeyedService<IInspector>(provider);
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace Schemby;

/// <summary>
/// Interface for provider installer.
/// </summary>
public interface IProviderInstaller
{
    /// <summary>
    /// Installs the provider into the specified service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="name">The provider name.</param>
    void Install(IServiceCollection services, string name);
}
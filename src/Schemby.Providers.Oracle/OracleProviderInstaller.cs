using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Schemby.Providers;

/// <summary>
/// Installer for the Oracle provider.
/// </summary>
public class OracleProviderInstaller : IProviderInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, string name)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (name == null) throw new ArgumentNullException(nameof(name));

        services.TryAddKeyedSingleton<IInspector, OracleInspector>(name);
    }
}
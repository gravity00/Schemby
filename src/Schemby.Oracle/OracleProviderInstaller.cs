using Microsoft.Extensions.DependencyInjection;

namespace Schemby;

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

        services.AddKeyedSingleton<IInspector, OracleInspector>(name);
    }
}
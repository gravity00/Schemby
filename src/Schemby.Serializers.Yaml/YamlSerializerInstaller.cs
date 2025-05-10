using Microsoft.Extensions.DependencyInjection;

namespace Schemby.Serializers;

/// <summary>
/// Installer for the Yaml serializer.
/// </summary>
public class YamlSerializerInstaller : ISerializerInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, string name)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        if (name is null) throw new ArgumentNullException(nameof(name));

        services.AddKeyedSingleton<ISerializer, YamlSerializer>(name);
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

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

        services.AddSingleton(s =>
        {
            var options = s.GetRequiredService<IOptions<YamlSerializerOptions>>().Value;

            var serializerBuilder = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .WithTypeConverter(new DateTimeOffsetConverter())
                .ConfigureDefaultValuesHandling(
                    DefaultValuesHandling.OmitDefaults | DefaultValuesHandling.OmitEmptyCollections
                )
                .DisableAliases();
            options.SerializerConfig?.Invoke(serializerBuilder);

            return serializerBuilder.Build();
        });
        services.AddKeyedSingleton<ISerializer, YamlSerializer>(name);
    }
}
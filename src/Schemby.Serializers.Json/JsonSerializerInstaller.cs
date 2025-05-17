using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Schemby.Serializers;

/// <summary>
/// Installer for the JSON serializer.
/// </summary>
public class JsonSerializerInstaller : ISerializerInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, string name)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        if (name is null) throw new ArgumentNullException(nameof(name));

        services.TryAddSingleton(s =>
        {
            var options = s.GetRequiredService<IOptions<JsonSerializerOptions>>().Value;
            var serializerOptions = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            options.SerializerConfig?.Invoke(serializerOptions);

            return serializerOptions;
        });
        services.TryAddKeyedSingleton<ISerializer, JsonSerializer>(name);
    }
}
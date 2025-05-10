using Schemby;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Schemby services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="options">Schemby configuration options</param>
    /// <returns>A reference to this instance after the operation has completed</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddSchemby(
        this IServiceCollection services,
        SchembyOptions options
    )
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        if (options is null) throw new ArgumentNullException(nameof(options));

        if (options.ProviderMapper is not null)
        {
            var providerMap = new Dictionary<string, IProviderInstaller>(
                StringComparer.OrdinalIgnoreCase
            );
            options.ProviderMapper(providerMap);

            foreach (var (name, installer) in providerMap.Select(e => (e.Key.ToLowerInvariant(), e.Value)))
                installer.Install(services, name);
        }

        if (options.SerializerMapper is not null)
        {
            var providerMap = new Dictionary<string, ISerializerInstaller>(
                StringComparer.OrdinalIgnoreCase
            );
            options.SerializerMapper(providerMap);

            foreach (var (name, installer) in providerMap.Select(e => (e.Key.ToLowerInvariant(), e.Value)))
                installer.Install(services, name);
        }

        services.AddSingleton<ISqlRunner, SqlRunner>();
        services.AddSingleton<IInspectorFactory, InspectorFactory>();
        services.AddSingleton<ISerializerFactory, SerializerFactory>();

        return services;
    }
}
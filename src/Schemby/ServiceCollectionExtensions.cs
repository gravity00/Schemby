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
    /// <param name="providerMapper">Callback to configure database providers</param>
    /// <returns>A reference to this instance after the operation has completed</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddSchemby(
        this IServiceCollection services,
        Action<IDictionary<string, IProviderInstaller>> providerMapper
    )
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (providerMapper == null) throw new ArgumentNullException(nameof(providerMapper));

        services.AddSingleton<ISqlRunner, SqlRunner>();
        services.AddSingleton<IInspectorFactory, InspectorFactory>();

        var providerMap = new Dictionary<string, IProviderInstaller>(
            StringComparer.OrdinalIgnoreCase
        );
        providerMapper(providerMap);

        foreach (var (name, installer) in providerMap.Select(e => (e.Key.ToLowerInvariant(), e.Value)))
            installer.Install(services, name);

        return services;
    }
}
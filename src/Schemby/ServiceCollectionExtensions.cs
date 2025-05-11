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

        services.AddSingleton<ISqlRunner, SqlRunner>();

        services.AddSingleton<IInspectorFactory, InspectorFactory>();
        foreach (var (name, installer) in options.Providers.Select(e => (e.Key.ToLowerInvariant(), e.Value)))
            installer.Install(services, name);

        services.AddSingleton<ISerializerFactory, SerializerFactory>();
        foreach (var (name, installer) in options.Serializers.Select(e => (e.Key.ToLowerInvariant(), e.Value)))
            installer.Install(services, name);

        return services;
    }

    /// <summary>
    /// Adds Schemby services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configure">Callback to configure Schemby options</param>
    /// <returns>A reference to this instance after the operation has completed</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddSchemby(
        this IServiceCollection services,
        Action<SchembyOptions> configure
    )
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        if (configure is null) throw new ArgumentNullException(nameof(configure));

        var options = new SchembyOptions();
        configure(options);
        return services.AddSchemby(options);
    }
}
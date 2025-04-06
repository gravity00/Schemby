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
    /// <returns>A reference to this instance after the operation has completed</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddSchemby(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<ISqlRunner, SqlRunner>();
        services.AddSingleton<IInspectorFactory, InspectorFactory>();

        return services;
    }
}
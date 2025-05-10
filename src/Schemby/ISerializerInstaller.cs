using Microsoft.Extensions.DependencyInjection;

namespace Schemby;

/// <summary>
/// Interface for serializer installer.
/// </summary>
public interface ISerializerInstaller
{
    /// <summary>
    /// Installs the serializer into the specified service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="name">The serializer name.</param>
    void Install(IServiceCollection services, string name);
}
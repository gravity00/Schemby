using Schemby.Serializers;

// ReSharper disable once CheckNamespace
namespace Schemby;

/// <summary>
/// Extensions for Schemby options.
/// </summary>
public static class SchembyOptionsExtensions
{
    /// <summary>
    /// Adds the JSON serializer to the Schemby options.
    /// </summary>
    /// <param name="options">Schemby options</param>
    /// <param name="name">The serializer name</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static SchembyOptions WithJsonSerializer(this SchembyOptions options, string name = "json")
    {
        if (options is null) throw new ArgumentNullException(nameof(options));
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be whitespace.", nameof(name));

        options.Serializers[name] = new JsonSerializerInstaller();
        return options;
    }
}
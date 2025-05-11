namespace Schemby;

/// <summary>
/// Options for Schemby.
/// </summary>
public class SchembyOptions
{
    /// <summary>
    /// Gets or sets a delegate to map database providers.
    /// </summary>
    public IDictionary<string, IProviderInstaller> Providers { get; } = new Dictionary<string, IProviderInstaller>(
        StringComparer.OrdinalIgnoreCase
    );

    /// <summary>
    /// Gets or sets a delegate to map serializers.
    /// </summary>
    public IDictionary<string, ISerializerInstaller> Serializers { get; } = new Dictionary<string, ISerializerInstaller>(
        StringComparer.OrdinalIgnoreCase
    );
}
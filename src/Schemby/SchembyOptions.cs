namespace Schemby;

/// <summary>
/// Options for Schemby.
/// </summary>
public class SchembyOptions
{
    /// <summary>
    /// Gets or sets a delegate to map database providers.
    /// </summary>
    public Action<IDictionary<string, IProviderInstaller>>? ProviderMapper { get; set; }

    /// <summary>
    /// Gets or sets a delegate to map serializers.
    /// </summary>
    public Action<IDictionary<string, ISerializerInstaller>>? SerializerMapper { get; set; }
}
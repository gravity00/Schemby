using System.Text;
using YamlDotNet.Serialization;

namespace Schemby.Serializers;

/// <summary>
/// YAML serializer options
/// </summary>
public class YamlSerializerOptions
{
    private Encoding _encoding = Encoding.UTF8;

    /// <summary>
    /// The encoding to be used.
    /// Defaults to <see cref="Encoding.UTF8"/>.
    /// </summary>
    public Encoding Encoding
    {
        get => _encoding;
        set => _encoding = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// The serializer configuration callback.
    /// </summary>
    public Action<SerializerBuilder>? SerializerConfig { get; set; }
}
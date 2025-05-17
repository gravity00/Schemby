using System.Text;

namespace Schemby.Serializers;

/// <summary>
/// JSON serializer options
/// </summary>
public class JsonSerializerOptions
{
    /// <summary>
    /// The serializer configuration callback.
    /// </summary>
    public Action<System.Text.Json.JsonSerializerOptions>? SerializerConfig { get; set; }
}
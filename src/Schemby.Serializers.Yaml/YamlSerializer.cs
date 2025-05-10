using System.Text;
using Schemby.Specifications;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;

namespace Schemby.Serializers;

/// <summary>
/// Serializes a database specification to YAML format.
/// </summary>
public class YamlSerializer : ISerializer
{
    private readonly YamlDotNet.Serialization.ISerializer _serializer;
    private readonly Encoding _encoding;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public YamlSerializer() : this(builder =>
        {
            builder
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .WithTypeConverter(new DateTimeOffsetConverter())
                .ConfigureDefaultValuesHandling(
                    DefaultValuesHandling.OmitNull | DefaultValuesHandling.OmitEmptyCollections
                )
                .DisableAliases();
        },
        Encoding.UTF8
    )
    {

    }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="serializerConfig">The serializer configuration callback</param>
    /// <param name="encoding">The encoding to use</param>
    public YamlSerializer(
        Action<SerializerBuilder> serializerConfig,
        Encoding encoding
    )
    {
        if (serializerConfig is null) throw new ArgumentNullException(nameof(serializerConfig));
        _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));

        var serializerBuilder = new SerializerBuilder();
        serializerConfig(serializerBuilder);
        _serializer = serializerBuilder.Build();
    }

    /// <inheritdoc />
    public async Task SerializeToStreamAsync(
        Specification specification,
        Stream stream,
        CancellationToken ct
    )
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));
        if (stream is null) throw new ArgumentNullException(nameof(stream));

#if !NET48
        await
#endif
            using var writer = new StreamWriter(stream, _encoding, 1024, true);

        _serializer.Serialize(writer, specification);

#if NET8_0
        await writer.FlushAsync(ct);
#else
        await writer.FlushAsync();
#endif
    }
}
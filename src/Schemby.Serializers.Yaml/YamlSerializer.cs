using Microsoft.Extensions.Options;
using Schemby.Specifications;

namespace Schemby.Serializers;

/// <summary>
/// Serializes a database specification to YAML format.
/// </summary>
/// <param name="options">The serializer options</param>
/// <param name="serializer">The YAML serializer instance</param>
public class YamlSerializer(
    IOptionsMonitor<YamlSerializerOptions> options,
    YamlDotNet.Serialization.ISerializer serializer
) : ISerializer
{
    private YamlSerializerOptions Options => options.CurrentValue;

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
            using var writer = new StreamWriter(stream, Options.Encoding, 1024, true);

        serializer.Serialize(writer, specification);

#if NET8_0
        await writer.FlushAsync(ct);
#else
        await writer.FlushAsync();
#endif
    }
}
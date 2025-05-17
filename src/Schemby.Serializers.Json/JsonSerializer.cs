using Schemby.Specifications;

namespace Schemby.Serializers;

/// <summary>
/// Serializes a database specification to JSON format.
/// </summary>
/// <param name="options">The serializer options</param>
public class JsonSerializer(
    System.Text.Json.JsonSerializerOptions options
) : ISerializer
{
    /// <inheritdoc />
    public async Task SerializeToStreamAsync(
        Specification specification,
        Stream stream,
        CancellationToken ct
    )
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));
        if (stream is null) throw new ArgumentNullException(nameof(stream));

        await System.Text.Json.JsonSerializer.SerializeAsync(
            stream,
            specification,
            options,
            ct
        );

        await stream.FlushAsync(ct);
    }
}
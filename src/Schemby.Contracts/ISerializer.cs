using Schemby.Specifications;

namespace Schemby;

/// <summary>
/// Interface for specification serializers.
/// </summary>
public interface ISerializer
{
    /// <summary>
    /// Serializes a specification into a stream.
    /// </summary>
    /// <param name="specification">The specification to serialize</param>
    /// <param name="stream">The destination stream</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns>A task to be awaited for the result</returns>
    Task SerializeToStreamAsync(
        Specification specification,
        Stream stream,
        CancellationToken ct
    );
}
namespace Schemby;

/// <summary>
/// Interface for specification serializer factories.
/// </summary>
public interface ISerializerFactory
{
    /// <summary>
    /// Creates a specification serializer.
    /// </summary>
    /// <param name="format">The serialization format</param>
    /// <returns>The specification serializer</returns>
    ISerializer Create(string format);
}
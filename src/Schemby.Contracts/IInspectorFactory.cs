namespace Schemby;

/// <summary>
/// Interface for database inspector factories.
/// </summary>
public interface IInspectorFactory
{
    /// <summary>
    /// Creates a database inspector.
    /// </summary>
    /// <param name="provider">The provider selector alias</param>
    /// <returns>The database inspector</returns>
    IInspector Create(string provider);
}
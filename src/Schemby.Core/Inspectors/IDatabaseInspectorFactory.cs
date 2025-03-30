namespace Schemby.Inspectors;

/// <summary>
/// Interface for database inspector factories.
/// </summary>
public interface IDatabaseInspectorFactory
{
    /// <summary>
    /// Creates a database inspector.
    /// </summary>
    /// <param name="provider">The provider selector alias</param>
    /// <returns>The database inspector</returns>
    IDatabaseInspector Create(
        string provider
    );
}
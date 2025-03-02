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
    /// <param name="connectionString">The connection string to be used by the inspector</param>
    /// <returns>The database inspector</returns>
    IDatabaseInspector Create(
        string provider,
        string connectionString
    );
}
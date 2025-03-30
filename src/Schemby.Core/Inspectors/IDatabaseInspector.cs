using Schemby.Specifications;

namespace Schemby.Inspectors;

/// <summary>
/// Interface for database inspectors.
/// </summary>
public interface IDatabaseInspector
{
    /// <summary>
    /// Inspects the database and returns a database specification.
    /// </summary>
    /// <param name="connectionString">The database connection string</param>
    /// <param name="database">The database name to inspect</param>
    /// <param name="options">Optional inspection options</param>
    /// <param name="ct">A token to cancel the operation</param>
    /// <returns>The database specification</returns>
    Task<Database> InspectAsync(
        string connectionString,
        string database,
        DatabaseInspectorOptions? options,
        CancellationToken ct
    );
}
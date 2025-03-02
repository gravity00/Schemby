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
    /// <param name="database">The database name to inspect</param>
    /// <param name="tableFilter">An optional table filter</param>
    /// <param name="columnFilter">An optional column filter</param>
    /// <param name="ct">A token to cancel the operation</param>
    /// <returns>The database specification</returns>
    Task<Database> InspectAsync(
        string database,
        string? tableFilter,
        string? columnFilter,
        CancellationToken ct
    );
}
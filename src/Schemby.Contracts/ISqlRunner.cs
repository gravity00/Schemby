using System.Data;

namespace Schemby;

/// <summary>
/// Executes SQL commands on a database connection.
/// </summary>
public interface ISqlRunner
{
    /// <summary>
    /// Executes a SQL command that returns a collection of items.
    /// </summary>
    /// <typeparam name="T">Collection item type</typeparam>
    /// <param name="connection">The database connection</param>
    /// <param name="sql">The SQL command to execute</param>
    /// <param name="options">The SQL command options</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns></returns>
    Task<IEnumerable<T>> QueryAsync<T>(
        IDbConnection connection,
        string sql,
        SqlOptions options,
        CancellationToken ct
    );
}
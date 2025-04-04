using System.Data;

namespace Schemby;

/// <summary>
/// Extensions for <see cref="ISqlRunner"/>.
/// </summary>
public static class SqlRunnerExtensions
{
    /// <summary>
    /// Executes a SQL command that returns a collection of items.
    /// </summary>
    /// <typeparam name="T">Collection item type</typeparam>
    /// <param name="runner">The SQL runner</param>
    /// <param name="connection">The database connection</param>
    /// <param name="sql">The SQL command to execute</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns></returns>
    public static Task<IEnumerable<T>> QueryAsync<T>(
        this ISqlRunner runner,
        IDbConnection connection,
        string sql,
        CancellationToken ct
    ) => runner.QueryAsync<T>(connection, sql, default, ct);

}
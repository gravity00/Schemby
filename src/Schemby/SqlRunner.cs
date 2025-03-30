using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Schemby;

/// <summary>
/// Executes SQL commands on a database connection.
/// </summary>
/// <param name="logger">The logger instance</param>
public class SqlRunner(
    ILogger<SqlRunner> logger
) : ISqlRunner
{
    /// <inheritdoc />
    public async Task<IEnumerable<T>> QueryAsync<T>(
        IDbConnection connection,
        string sql,
        SqlOptions options,
        CancellationToken ct
    )
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection));
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        var command = BuildCommand(sql, options, ct);
        Log(command);
        return await connection.QueryAsync<T>(command).ConfigureAwait(false);
    }

    private static CommandDefinition BuildCommand(
        string sql,
        SqlOptions options,
        CancellationToken ct
    ) => new(sql, options.Parameters, commandTimeout: options.Timeout, cancellationToken: ct);

    private void Log(CommandDefinition command) => logger.LogDebug(@"Executing SQL command
[HasParams:{HasParameters} Timeout:{Timeout}] {Sql}",
        command.Parameters is not null,
        command.CommandTimeout,
        command.CommandText
    );
}
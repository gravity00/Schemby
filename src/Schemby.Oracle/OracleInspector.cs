using System.Collections.Concurrent;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Schemby.Entities;
using Schemby.Specifications;

namespace Schemby;

/// <summary>
/// Inspects an Oracle database.
/// </summary>
/// <param name="logger">The inspector logger</param>
/// <param name="sqlRunner">The SQL runner</param>
public class OracleInspector(
    ILogger<OracleInspector> logger,
    ISqlRunner sqlRunner
) : IInspector
{
    private static readonly ConcurrentDictionary<(
        string Type,
        int Length,
        int? Precision,
        int? Scale
    ), (
        ColumnType Type,
        string TypeNative
    )> TypeMap = new();

    /// <inheritdoc />
    public async Task<Database> InspectAsync(
        string connectionString,
        string database,
        InspectOptions? options,
        CancellationToken ct
    )
    {
        if (connectionString is null) throw new ArgumentNullException(nameof(connectionString));
        if (database is null) throw new ArgumentNullException(nameof(database));

        using var _ = logger.BeginScope("Database:'{Database}'", database);

        logger.LogDebug("Connecting to database");
#if !NET48
        await
#endif
            using var db = await ConnectAsync(connectionString, ct);

        logger.LogDebug("Inspecting database columns [{DatabaseInspectorOptions}]", options);
        var dbColumns = await GetColumnsAsync(
            db,
            database,
            options ?? default,
            ct
        );

        var tables = dbColumns.GroupBy(c => c.TableName).Select(g =>
        {
            var columns = g.Select(c =>
            {
                var (type, rawType) = MapColumnType(c.Type, c.Length, c.Precision, c.Scale);
                return new Column(
                    c.ColumnName,
                    type,
                    rawType,
                    c.Nullable,
                    c.Length
                )
                {
                    Description = c.Description,
                    Precision = c.Precision,
                    Scale = c.Scale,
                    DefaultValue = c.DefaultValue
                };
            }).ToArray();

            return new Table(
                g.Key,
                columns
            );
        }).ToArray();

        return new Database(
            new Metadata(1, DateTimeOffset.UtcNow),
            database,
            tables
        );
    }

    private async Task<IEnumerable<TableColumnViewEntity>> GetColumnsAsync(
        IDbConnection connection,
        string database,
        InspectOptions options,
        CancellationToken ct
    )
    {
        var sqlBuilder = new StringBuilder(@"
SELECT 
    T.OWNER DatabaseName,
    T.TABLE_NAME TableName,
    C.COLUMN_NAME ColumnName,
    C.DATA_TYPE Type,
    DECODE(C.NULLABLE, 
        'Y', 1, 
        'N', 0
    ) Nullable,
    C.DATA_LENGTH Length,
    C.DATA_PRECISION Precision,
    C.DATA_SCALE Scale,
    C.DATA_DEFAULT DefaultValue,
    CM.COMMENTS Description
FROM ALL_TABLES T
INNER JOIN ALL_TAB_COLUMNS C
    ON T.TABLE_NAME = C.TABLE_NAME AND T.OWNER = C.OWNER
LEFT JOIN ALL_COL_COMMENTS CM 
    ON C.OWNER = CM.OWNER AND C.TABLE_NAME = CM.TABLE_NAME AND C.COLUMN_NAME = CM.COLUMN_NAME
WHERE
    T.OWNER = :Database");

        if (!string.IsNullOrWhiteSpace(options.TableFilter))
            sqlBuilder.Append(@"
    AND T.TABLE_NAME LIKE :TableFilter");

        if (!string.IsNullOrWhiteSpace(options.ColumnFilter))
            sqlBuilder.Append(@"
    AND C.COLUMN_NAME LIKE :ColumnFilter");

        sqlBuilder.Append(@"
ORDER BY
    T.OWNER,
    T.TABLE_NAME,
    C.COLUMN_ID");

        return await sqlRunner.QueryAsync<TableColumnViewEntity>(
            connection,
            sqlBuilder.ToString(),
            new SqlOptions
            {
                Parameters = new
                {
                    Database = database.ToUpperInvariant(),
                    options.TableFilter,
                    options.ColumnFilter
                }
            },
            ct
        );
    }

    private static async Task<OracleConnection> ConnectAsync(
        string connectionString,
        CancellationToken ct
    )
    {
        var connection = new OracleConnection(connectionString);
        try
        {
            await connection.OpenAsync(ct).ConfigureAwait(false);
        }
        catch
        {
#if NET48
            connection.Dispose();
#else
            await connection.DisposeAsync().ConfigureAwait(false);
#endif
            throw;
        }

        return connection;
    }

    private static (ColumnType Type, string TypeNative) MapColumnType(
        string columnType,
        int columnLength,
        int? columnPrecision,
        int? columnScale
    ) => TypeMap.GetOrAdd((columnType, columnLength, columnPrecision, columnScale), x =>
    {
        var (type, length, precision, scale) = x;
        return type switch
        {
            "VARCHAR2" or "NVARCHAR2" or "CHAR" or "NCHAR" => (
                length is 1 ? ColumnType.Char : ColumnType.Text,
                $"{type}({length})"
            ),
            "CLOB" or "NCLOB" or "LONG" => (
                ColumnType.TextLarge,
                type
            ),

            "NUMBER" => (
                scale is > 0 ? ColumnType.Decimal :
                precision > 9 ? ColumnType.Long :
                ColumnType.Integer,
                $"NUMBER({precision},{scale})"
            ),
            "FLOAT" => (
                ColumnType.Decimal,
                $"FLOAT({precision})"
            ),
            "BINARY_FLOAT" => (
                ColumnType.Float,
                "BINARY_FLOAT"
            ),
            "BINARY_DOUBLE" => (
                ColumnType.Double,
                "BINARY_DOUBLE"
            ),

            "DATE" => (
                ColumnType.DateTime,
                "DATE"
            ),

            "RAW" => (
                length is 16 ? ColumnType.Uuid : ColumnType.Binary,
                $"RAW({length})"
            ),
            "BLOB" or "LONG RAW" => (
                ColumnType.BinaryLarge,
                type
            ),

            _ => (
                IsTimestamp(type) ? ColumnType.DateTime :
                IsTimestampWithTimezone(type) ? ColumnType.DateTimeWithTimezone :
                IsIntervalYearToMonth(type) ? ColumnType.TimeInterval :
                IsIntervalDayToSecond(type) ? ColumnType.TimeInterval :
                ColumnType.Undefined,
                type
            )
        };
    });

    private static bool IsTimestamp(string type) => Regex.IsMatch(
        type,
        @"TIMESTAMP\(\d\)"
    );

    private static bool IsTimestampWithTimezone(string type) => Regex.IsMatch(
        type,
        @"TIMESTAMP\(\d\) WITH TIME ZONE"
    );

    private static bool IsIntervalYearToMonth(string type) => Regex.IsMatch(
        type,
        @"INTERVAL YEAR\(\d\) TO MONTH"
    );

    private static bool IsIntervalDayToSecond(string type) => Regex.IsMatch(
        type,
        @"INTERVAL DAY\(\d\) TO SECOND\(\d\)"
    );
}
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Schemby.Specifications;
using Index = Schemby.Specifications.Index;

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

        database = database.ToUpperInvariant();
        using var _ = logger.BeginScope("Database:'{Database}'", database);

        logger.LogDebug("Connecting to database [{DatabaseInspectorOptions}]", options);
#if !NET48
        await
#endif
            using var db = await ConnectAsync(connectionString, ct);

        var tableFilter = options?.TableFilter;
        var columnFilter = options?.ColumnFilter;

        logger.LogDebug("Inspecting database columns");
        var dbColumns = await sqlRunner.GetColumnsAsync(
            db,
            database,
            tableFilter,
            columnFilter,
            ct
        );

        logger.LogDebug("Inspect database primary keys");
        var dbPrimaryKeys = (await sqlRunner.GetPrimaryKeysAsync(
            db,
            database,
            tableFilter,
            ct
        )).GroupBy(e => e.TableName).ToDictionary(e => e.Key);

        logger.LogDebug("Inspecting database indexes");
        var dbIndexes = (await sqlRunner.GetIndexesAsync(
            db,
            database,
            tableFilter,
            columnFilter,
            ct
        )).GroupBy(e => e.TableName).ToDictionary(e => e.Key);

        var tables = dbColumns.GroupBy(c => c.TableName).Select(g =>
        {

            PrimaryKey? primaryKey;
            if (dbPrimaryKeys.TryGetValue(g.Key, out var dbTablePrimaryKeys))
            {
                var dbTablePrimaryKey = dbTablePrimaryKeys.First();
                primaryKey = new PrimaryKey(
                    dbTablePrimaryKey.ConstraintName,
                    dbTablePrimaryKeys.Select(i => i.ColumnName).ToArray()
                );
            }
            else
                primaryKey = null;

            var columns = g.Select(c =>
            {
                var (type, rawType) = MapColumnType(c.Type, c.Length, c.Precision, c.Scale);
                return new Column(
                    c.ColumnName,
                    type,
                    rawType,
                    c.IsNullable,
                    c.Length
                )
                {
                    Description = c.Description,
                    Precision = c.Precision,
                    Scale = c.Scale,
                    DefaultValue = c.DefaultValue
                };
            }).ToArray();

            Index[] indexes;
            if (dbIndexes.TryGetValue(g.Key, out var dbTableIndexes))
            {
                indexes = dbTableIndexes.GroupBy(i => i.IndexName).Select(ig =>
                {
                    var dbIndex = ig.First();
                    return new Index(
                        dbIndex.IndexName,
                        dbIndex.IsUnique,
                        ig.Select(i => new IndexColumn(
                            i.ColumnName,
                            i.IsAscending
                        )).ToArray()
                    );
                }).ToArray();
            }
            else
                indexes = [];

            return new Table(
                g.Key,
                columns
            )
            {
                PrimaryKey = primaryKey,
                Indexes = indexes
            };
        }).ToArray();

        return new Database(
            new Metadata(1, DateTimeOffset.UtcNow),
            database,
            tables
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
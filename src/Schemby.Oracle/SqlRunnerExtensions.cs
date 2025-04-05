using System.Data;
using System.Text;
using Schemby.Entities;
// ReSharper disable UseRawString

namespace Schemby;

internal static class SqlRunnerExtensions
{
    public static async Task<IEnumerable<TableColumnViewEntity>> GetColumnsAsync(
        this ISqlRunner sqlRunner,
        IDbConnection connection,
        string database,
        string? tableFilter,
        string? columnFilter,
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
    ) IsNullable,
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

        if (!string.IsNullOrWhiteSpace(tableFilter))
            sqlBuilder.Append(@"
    AND T.TABLE_NAME LIKE :TableFilter");

        if (!string.IsNullOrWhiteSpace(columnFilter))
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
                    Database = database,
                    TableFilter = tableFilter,
                    ColumnFilter = columnFilter
                }
            },
            ct
        );
    }

    public static async Task<IEnumerable<TableIndexViewEntity>> GetIndexesAsync(
        this ISqlRunner sqlRunner,
        IDbConnection connection,
        string database,
        string? tableFilter,
        string? columnFilter,
        CancellationToken ct
    )
    {
        var sqlBuilder = new StringBuilder(@"
SELECT 
    I.OWNER DatabaseName,
    I.TABLE_NAME TableName,
    I.INDEX_NAME IndexName,
    IC.COLUMN_NAME ColumnName,
    DECODE(I.UNIQUENESS,
        'UNIQUE', 1,
        0
    ) IsUnique,
    DECODE(IC.DESCEND,
        'ASC', 1,
        0
    ) IsAscending
FROM ALL_INDEXES I
INNER JOIN ALL_IND_COLUMNS IC
    ON I.INDEX_NAME = IC.INDEX_NAME
WHERE
    I.OWNER = :Database");

        if (!string.IsNullOrWhiteSpace(tableFilter))
            sqlBuilder.Append(@"
    AND I.TABLE_NAME LIKE :TableFilter");

        if (!string.IsNullOrWhiteSpace(columnFilter))
            sqlBuilder.Append(@"
    AND IC.COLUMN_NAME LIKE :ColumnFilter");

        sqlBuilder.Append(@"
ORDER BY
    I.OWNER,
    I.TABLE_NAME,
    IC.COLUMN_POSITION");

        return await sqlRunner.QueryAsync<TableIndexViewEntity>(
            connection,
            sqlBuilder.ToString(),
            new SqlOptions
            {
                Parameters = new
                {
                    Database = database,
                    TableFilter = tableFilter,
                    ColumnFilter = columnFilter
                }
            },
            ct
        );
    }
}
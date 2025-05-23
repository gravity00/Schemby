﻿using System.Data;
using System.Text;
using Schemby.Providers.Entities;

// ReSharper disable UseRawString

namespace Schemby.Providers;

internal static class SqlRunnerExtensions
{
    public static async Task<IEnumerable<TableColumnViewEntity>> GetColumnsAsync(
        this ISqlRunner sqlRunner,
        IDbConnection connection,
        string database,
        string? tableFilter,
        bool isExclusiveTableFilter,
        string? columnFilter,
        bool isExclusiveColumnFilter,
        CancellationToken ct
    )
    {
        var sqlBuilder = new StringBuilder(@"
SELECT
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
    ON T.TABLE_NAME = C.TABLE_NAME
        AND T.OWNER = C.OWNER
LEFT JOIN ALL_COL_COMMENTS CM
    ON C.OWNER = CM.OWNER
        AND C.TABLE_NAME = CM.TABLE_NAME
        AND C.COLUMN_NAME = CM.COLUMN_NAME
WHERE
    T.OWNER = :Database");

        if (!string.IsNullOrWhiteSpace(tableFilter))
            sqlBuilder.Append(@"
    AND").AppendIf(isExclusiveTableFilter, " NOT").Append(" REGEXP_LIKE(T.TABLE_NAME, :TableFilter)");

        if (!string.IsNullOrWhiteSpace(columnFilter))
            sqlBuilder.Append(@"
    AND").AppendIf(isExclusiveColumnFilter, " NOT").Append(" REGEXP_LIKE(C.COLUMN_NAME, :ColumnFilter)");

        sqlBuilder.Append(@"
ORDER BY
    NLSSORT(T.OWNER, 'NLS_SORT = BINARY'),
    NLSSORT(T.TABLE_NAME, 'NLS_SORT = BINARY'),
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

    public static async Task<IEnumerable<TableConstraintViewEntity>> GetConstraintsAsync(
        this ISqlRunner sqlRunner,
        IDbConnection connection,
        string database,
        string? tableFilter,
        bool isExclusiveTableFilter,
        CancellationToken ct
    )
    {
        var sqlBuilder = new StringBuilder(@"
SELECT
    C.CONSTRAINT_NAME Name,
    ASCII(C.CONSTRAINT_TYPE) Type,
    C.TABLE_NAME TableName,
    CC.COLUMN_NAME ColumnName,
    C.R_CONSTRAINT_NAME ConstraintNameFk
FROM ALL_CONSTRAINTS C
INNER JOIN ALL_CONS_COLUMNS CC
    ON C.OWNER = CC.OWNER
        AND C.TABLE_NAME = CC.TABLE_NAME
        AND C.CONSTRAINT_NAME = CC.CONSTRAINT_NAME
WHERE
    C.CONSTRAINT_TYPE IN ('P', 'R')
    AND C.STATUS = 'ENABLED'
    AND C.OWNER = :Database
    AND (
        C.R_OWNER IS NULL
        OR C.R_OWNER = :Database
    )");

        if (!string.IsNullOrWhiteSpace(tableFilter))
            sqlBuilder.Append(@"
    AND").AppendIf(isExclusiveTableFilter, " NOT").Append(" REGEXP_LIKE(C.TABLE_NAME, :TableFilter)");

        sqlBuilder.Append(@"
ORDER BY
    NLSSORT(C.OWNER, 'NLS_SORT = BINARY'),
    NLSSORT(C.TABLE_NAME, 'NLS_SORT = BINARY'),
    NLSSORT(C.CONSTRAINT_NAME, 'NLS_SORT = BINARY'),
    CC.POSITION");

        return await sqlRunner.QueryAsync<TableConstraintViewEntity>(
            connection,
            sqlBuilder.ToString(),
            new SqlOptions
            {
                Parameters = new
                {
                    Database = database,
                    TableFilter = tableFilter
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
        bool isExclusiveTableFilter,
        string? columnFilter,
        bool isExclusiveColumnFilter,
        CancellationToken ct
    )
    {
        var sqlBuilder = new StringBuilder(@"
SELECT 
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
    ON I.OWNER = IC.INDEX_OWNER
        AND I.INDEX_NAME = IC.INDEX_NAME
WHERE
    I.OWNER = :Database");

        if (!string.IsNullOrWhiteSpace(tableFilter))
            sqlBuilder.Append(@"
    AND").AppendIf(isExclusiveTableFilter, " NOT").Append(" REGEXP_LIKE(I.TABLE_NAME, :TableFilter)");

        if (!string.IsNullOrWhiteSpace(columnFilter))
            sqlBuilder.Append(@"
    AND").AppendIf(isExclusiveColumnFilter, " NOT").Append(" REGEXP_LIKE(IC.COLUMN_NAME, :ColumnFilter)");

        sqlBuilder.Append(@"
ORDER BY
    NLSSORT(I.OWNER, 'NLS_SORT = BINARY'),
    NLSSORT(I.TABLE_NAME, 'NLS_SORT = BINARY'),
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

    private static StringBuilder AppendIf(
        this StringBuilder sb,
        bool condition,
        string value
    )
    {
        if(condition)
            sb.Append(value);
        return sb;
    }
}
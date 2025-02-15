﻿namespace Schemby.Specifications;

/// <summary>
/// Represents a table specification.
/// </summary>
/// <param name="Name">The table name</param>
/// <param name="Columns">Collection of column specifications</param>
/// <param name="Description">Optional specification description</param>
public record Table(
    string Name,
    IEnumerable<Column> Columns,
    string? Description = null
);
﻿namespace Schemby.Specifications;

/// <summary>
/// Represents a column specification.
/// </summary>
/// <param name="Name">The column name</param>
/// <param name="Type">The column type</param>
public record Column(
    string Name,
    ColumnType Type
)
{
    /// <summary>
    /// Specification description.
    /// </summary>
    public string? Description { get; init; }
}
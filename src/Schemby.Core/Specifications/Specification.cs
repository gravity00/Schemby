namespace Schemby.Specifications;

/// <summary>
/// Represents a collection of database specifications.
/// </summary>
/// <param name="Version">The specification version</param>
/// <param name="Databases">Collection of database specifications</param>
/// <param name="Description">Optional specification description</param>
public record Specification(
    int Version,
    IEnumerable<Database> Databases,
    string? Description = null
);
namespace Schemby.Specifications;

/// <summary>
/// Represents a column type specification.
/// </summary>
public enum ColumnType
{
    /// <summary>
    /// Represents an undefined column type.
    /// </summary>
    Undefined,

    /// <summary>
    /// Represents a boolean column type.
    /// </summary>
    Bool,

    #region Text types

    /// <summary>
    /// Represents a single character column type.
    /// </summary>
    Char,
    /// <summary>
    /// Represents a multiple character column type.
    /// </summary>
    Text,
    /// <summary>
    /// Represents a multiple character column type.
    /// </summary>
    TextLarge,

    #endregion

    #region Number types

    /// <summary>
    /// Represents an integer column type.
    /// </summary>
    Integer,
    /// <summary>
    /// Represents a long integer column type.
    /// </summary>
    Long,
    /// <summary>
    /// Represents a single-precision floating point column type.
    /// </summary>
    Float,
    /// <summary>
    /// Represents a double-precision floating point column type.
    /// </summary>
    Double,
    /// <summary>
    /// Represents a decimal column type.
    /// </summary>
    Decimal,

    #endregion

    #region Date and Time types

    /// <summary>
    /// Represents a time column type.
    /// </summary>
    Time,
    /// <summary>
    /// Represents a time interval column type.
    /// </summary>
    TimeInterval,
    /// <summary>
    /// Represents a date column type.
    /// </summary>
    Date,
    /// <summary>
    /// Represents a date and time column type.
    /// </summary>
    DateTime,
    /// <summary>
    /// Represents a date and time with offset column type.
    /// </summary>
    DateTimeWithTimezone,

    #endregion

    #region Raw types

    /// <summary>
    /// Represents a binary data column type.
    /// </summary>
    Binary,
    /// <summary>
    /// Represents a binary data column type.
    /// </summary>
    BinaryLarge,
    /// <summary>
    /// Represents a universally unique identifier column type.
    /// </summary>
    Uuid,

    #endregion

    /// <summary>
    /// Represents a custom column type.
    /// </summary>
    Custom
}
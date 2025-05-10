namespace Schemby.Providers.Entities;

internal enum TableConstraintTypeEntity
{
    Undefined = 0,

    PrimaryKey = 'P',
    ForeignKey = 'R',
    UniqueKey = 'U',
    Check = 'C',
}
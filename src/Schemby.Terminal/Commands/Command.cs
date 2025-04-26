namespace Schemby.Commands;

public record Command : ICommand
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTimeOffset CreatedOn { get; } = DateTimeOffset.UtcNow;

    public string CreatedBy { get; } = Environment.UserName;

    public bool Verbose { get; init; }
}
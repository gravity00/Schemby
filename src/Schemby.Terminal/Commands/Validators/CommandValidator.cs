using FluentValidation;

namespace Schemby.Commands.Validators;

public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>
    where TCommand : Command
{
    protected CommandValidator()
    {
        RuleFor(x => x.Verbose);
    }
}
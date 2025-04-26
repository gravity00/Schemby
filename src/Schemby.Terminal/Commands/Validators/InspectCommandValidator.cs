using FluentValidation;

namespace Schemby.Commands.Validators;

public class InspectCommandValidator : CommandValidator<InspectCommand>
{
    public InspectCommandValidator()
    {
        RuleFor(x => x.ConnectionString)
            .NotEmpty();
        RuleFor(x => x.Provider)
            .NotEmpty();
        RuleFor(x => x.Database)
            .NotEmpty();
        RuleFor(x => x.Output)
            .NotEmpty();
        RuleFor(x => x.TableFilter);
        RuleFor(x => x.ColumnFilter);
        RuleFor(x => x.Format)
            .NotEmpty()
            .IsInEnum();
    }
}
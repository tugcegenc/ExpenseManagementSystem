using Expense.Schema.Requests;
using FluentValidation;

namespace Expense.Application.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be valid.")
            .MaximumLength(100);

        RuleFor(x => x.IBAN)
            .NotEmpty().WithMessage("IBAN is required.")
            .Matches(@"^TR\d{24}$")
            .WithMessage("IBAN must start with 'TR' followed by 24 digits.");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Role must be either Admin or Personnel.");
    }
}


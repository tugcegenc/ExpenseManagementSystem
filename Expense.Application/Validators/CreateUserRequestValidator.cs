using Expense.Schema.Requests;
using FluentValidation;

namespace Expense.Application.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50);

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50);

        RuleFor(x => x.IdentityNumber)
            .NotEmpty().WithMessage("Identity number is required.")
            .Matches(@"^[1-9][0-9]{10}$")
            .WithMessage("Identity number must be 11 digits and cannot start with 0.");

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


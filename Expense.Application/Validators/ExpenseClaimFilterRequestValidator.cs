using Expense.Schema.Requests;
using FluentValidation;

namespace Expense.Application.Validators;

public class ExpenseClaimFilterRequestValidator : AbstractValidator<ExpenseClaimFilterRequest>
{
    public ExpenseClaimFilterRequestValidator()
    {
        RuleFor(x => x)
            .Must(x => !(x.MinAmount.HasValue && x.MaxAmount.HasValue) || x.MinAmount <= x.MaxAmount)
            .WithMessage("MinAmount cannot be greater than MaxAmount.");

        RuleFor(x => x)
            .Must(x => !(x.StartDate.HasValue && x.EndDate.HasValue) || x.StartDate <= x.EndDate)
            .WithMessage("StartDate cannot be later than EndDate.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("StartDate cannot be greater than EndDate");

        RuleFor(x => x.MinAmount)
            .GreaterThanOrEqualTo(0).When(x => x.MinAmount.HasValue)
            .WithMessage("MinAmount must be 0 or greater.");

        RuleFor(x => x.MaxAmount)
            .GreaterThan(0).When(x => x.MaxAmount.HasValue)
            .WithMessage("MaxAmount must be greater than 0.");

        RuleFor(x => x.ExpenseCategoryId)
            .GreaterThan(0).When(x => x.ExpenseCategoryId.HasValue)
            .WithMessage("ExpenseCategoryId must be a valid ID.");

        RuleFor(x => x.Status)
            .IsInEnum().When(x => x.Status.HasValue)
            .WithMessage("Status value is invalid.");
    }
}

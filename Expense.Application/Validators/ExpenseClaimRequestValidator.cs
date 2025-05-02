using Expense.Schema.Requests;
using FluentValidation;

namespace Expense.Application.Validators;

public class ExpenseClaimRequestValidator : AbstractValidator<ExpenseClaimRequest>
{
    public ExpenseClaimRequestValidator()
    {
        RuleFor(x => x.ExpenseCategoryId)
            .NotEmpty().WithMessage("Expense category ID is required.")
            .GreaterThan(0).WithMessage("Expense category ID must be greater than 0.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(100).WithMessage("Location must be less than 100 characters.");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment method.");

        RuleFor(x => x.ReceiptFile)
            .NotNull().WithMessage("Receipt file is required.")
            .Must(file => file.Length > 0).WithMessage("Receipt file cannot be empty.")
            .Must(file => file.Length <= 5 * 1024 * 1024)
                .WithMessage("Receipt file size must be less than 5 MB.")
            .Must(file => file.ContentType == "image/jpeg/jpg" || file.ContentType == "image/png")
                .WithMessage("Receipt file must be a JPEG,JPG or PNG image.");
    }
}

using Expense.Domain.Enums;

namespace Expense.Schema.Requests;

public class ExpenseClaimFilterRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public long? ExpenseCategoryId { get; set; }
    public ExpenseStatus? Status { get; set; }
}

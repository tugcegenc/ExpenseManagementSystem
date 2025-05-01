using Expense.Domain.Base;
using Expense.Schema.Base;

namespace Expense.Schema.Responses;

public class ExpenseCategoryResponse :  BaseResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
}
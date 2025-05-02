using Expense.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Expense.Schema.Requests;

public class ExpenseClaimRequest
{
    public long ExpenseCategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Location { get; set; } 
    public IFormFile? ReceiptFile { get; set; }
}

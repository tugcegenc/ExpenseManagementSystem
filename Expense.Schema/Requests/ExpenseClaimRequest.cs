using Expense.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Expense.Schema.Requests;

public class CreateExpenseClaimRequest
{
    public long ExpenseCategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Location { get; set; } 
    public IFormFile? ReceiptFile { get; set; }
}

public class UpdateExpenseClaimRequest
{
    public long Id { get; set; }
    public long ExpenseCategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Location { get; set; }
    public string? ReceiptFile { get; set; }
}
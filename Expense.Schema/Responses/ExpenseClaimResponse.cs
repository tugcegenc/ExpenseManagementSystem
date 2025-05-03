using System.Text.Json.Serialization;
using Expense.Domain.Enums;
using Expense.Schema.Base;

namespace Expense.Schema.Responses;

public class ExpenseClaimResponse : BaseResponse 
{
    public long UserId { get; set; }
    public long ExpenseCategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string Location { get; set; }
    public string ReceiptFileUrl { get; set; }
    public ExpenseStatus Status { get; set; }
    public string? RejectReason { get; set; }
    public DateTime ClaimDate { get; set; }
    public DateTime? ApprovedOrRejectedDate { get; set; }
    
    [JsonIgnore]
    public string? ReceiptFilePath { get; set; } 
}
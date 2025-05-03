using System.ComponentModel.DataAnnotations.Schema;
using Expense.Domain.Base;
using Expense.Domain.Enums;

namespace Expense.Domain.Entities;

[Table("ExpenseClaims", Schema = "dbo")]
public class ExpenseClaim : BaseEntity
{
    public long UserId { get; set; }
    public User User { get; set; }
    public long ExpenseCategoryId { get; set; }
    public ExpenseCategory ExpenseCategory { get; set; }
    public decimal Amount { get; set; }
    public DateTime ClaimDate { get; set; }
    public string Description { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string Location { get; set; } 
    public string? ReceiptFilePath { get; set; }
    public ExpenseStatus Status { get; set; }
    public string? RejectReason { get; set; } 
    public DateTime? ApprovedOrRejectedDate { get; set; }

    public virtual ICollection<EftSimulationLog> EftSimulationLogs { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using Expense.Domain.Base;

namespace Expense.Domain.Entities;

[Table("EftSimulationLogs", Schema = "dbo")]
public class EftSimulationLog : BaseEntity
{
    public long ExpenseClaimId { get; set; }
    public virtual ExpenseClaim ExpenseClaim { get; set; }
    public decimal Amount { get; set; }
    public string ReceiverIban { get; set; }
    public string ReceiverName { get; set; } 
    public string Description { get; set; }
    public DateTime SimulatedAt { get; set; }
    public bool IsSuccessful { get; set; }
}

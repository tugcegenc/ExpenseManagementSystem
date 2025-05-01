using Expense.Schema.Base;

namespace Expense.Schema.Responses;

public class EftSimulationLogResponse : BaseResponse
{
    public long ExpenseClaimId { get; set; }
    public decimal Amount { get; set; }
    public string ReceiverName { get; set; }
    public string Description { get; set; }
    public DateTime SimulatedAt { get; set; }
    public bool IsProcessed { get; set; }
}
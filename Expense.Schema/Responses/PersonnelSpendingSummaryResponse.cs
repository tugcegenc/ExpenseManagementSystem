namespace Expense.Schema.Responses;

public class PersonnelSpendingSummaryResponse
{
    public long UserId { get; set; }
    public string UserName { get; set; }
    public string Date { get; set; }
    public decimal TotalAmount { get; set; }
}
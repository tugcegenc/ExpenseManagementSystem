namespace Expense.Schema.Responses;

public class ReportSummaryResponse
{
    public string Period { get; set; } 
    public string Status { get; set; } 
    public decimal TotalAmount { get; set; }
}

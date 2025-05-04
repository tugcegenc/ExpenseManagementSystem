namespace Expense.Schema.Reports;
public class CompanyExpenseSummaryResponse
{
    public string PeriodType { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalClaimCount { get; set; }
    public decimal TotalClaimAmount { get; set; }
    public int ApprovedCount { get; set; }
    public decimal ApprovedAmount { get; set; }
    public int RejectedCount { get; set; }
    public decimal RejectedAmount { get; set; }
    public int PendingCount { get; set; }
    public decimal PendingAmount { get; set; }
}
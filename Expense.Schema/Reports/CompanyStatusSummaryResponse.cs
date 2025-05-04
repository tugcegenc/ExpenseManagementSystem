namespace Expense.Schema.Reports;

public class CompanyStatusSummaryResponse
{
    public string PeriodType { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public long ApprovedCount { get; set; }
    public decimal ApprovedAmount { get; set; }
    public long RejectedCount { get; set; }
    public decimal RejectedAmount { get; set; }
    
}



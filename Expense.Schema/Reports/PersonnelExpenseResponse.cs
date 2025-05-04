namespace Expense.Schema.Reports;

public class PersonnelExpenseResponse
{ 
    public string CategoryName { get; set; }                  
    public decimal Amount { get; set; }                       
    public string Status { get; set; }                       
    public DateTime ClaimDate { get; set; }                
    public DateTime? ApprovedOrRejectedDate { get; set; }   
    public string Description { get; set; }                 
    public string? RejectReason { get; set; }                 
}

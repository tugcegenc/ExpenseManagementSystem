namespace Expense.Schema.Responses;

public class PersonnelExpenseResponse
{
    public string CategoryName { get; set; }                  
    public decimal Amount { get; set; }                       
    public string Status { get; set; }                       
    public DateTime RequestDate { get; set; }                
    public DateTime? ApprovedOrRejectedDate { get; set; }   
    public string Description { get; set; }                 
    public string? RejectReason { get; set; }                 
}

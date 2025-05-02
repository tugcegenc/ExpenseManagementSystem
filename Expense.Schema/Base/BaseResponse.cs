namespace Expense.Schema.Base;

public abstract class BaseResponse
{
    public long Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive{ get; set; }
}
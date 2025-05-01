namespace Expense.Domain.Base;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public string CreatedBy { get; set; } 
    public DateTime CreatedAt { get; set; } 
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

}

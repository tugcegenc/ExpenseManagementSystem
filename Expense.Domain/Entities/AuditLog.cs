using System.ComponentModel.DataAnnotations.Schema;

namespace Expense.Domain.Entities;

[Table("AuditLog", Schema = "dbo")]
public class AuditLog 
{
    public long Id { get; set; } 
    public string UserName { get; set; }
    public string Role { get; set; }
    public string Action { get; set; } 
    public string EntityId { get; set; } 
    public string EntityName { get; set; } 
    public string OriginalValues { get; set; }
    public string ChangedValues { get; set; }
    public DateTime? Timestamp { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using Expense.Domain.Base;

namespace Expense.Domain.Entities;

[Table("ExpenseCategories", Schema = "dbo")]
public class ExpenseCategory : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public virtual ICollection<ExpenseClaim> ExpenseClaims { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using Expense.Domain.Base;
using Expense.Domain.Enums;

namespace Expense.Domain.Entities;

[Table("Users", Schema = "dbo")]
public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public string IdentityNumber { get; set; } 
    public string UserName { get; set; } 
    public string PasswordHash { get; set; } 
    public string PasswordSalt { get; set; }
    public string Email { get; set; } 
    public string IBAN { get; set; }
    public UserRole Role { get; set; }

    public virtual ICollection<ExpenseClaim> ExpenseClaims { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
}

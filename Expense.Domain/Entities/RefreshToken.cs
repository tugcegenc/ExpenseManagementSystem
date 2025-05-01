using System.ComponentModel.DataAnnotations.Schema;
using Expense.Domain.Base;

namespace Expense.Domain.Entities;

[Table("RefreshTokens", Schema = "dbo")]
public class RefreshToken : BaseEntity
{
    public long UserId { get; set; }
    public User User { get; set; }
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsUsed { get; set; } 
    public bool IsRevoked { get; set; } 
}
using Expense.Domain.Enums;
using Expense.Schema.Base;

namespace Expense.Schema.Responses;

public class UserResponse : BaseResponse
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string IdentityNumber { get; set; }
    public string UserName { get; set; }      
    public string Email { get; set; } 
    public string IBAN { get; set; }
    public UserRole Role { get; set; }
}
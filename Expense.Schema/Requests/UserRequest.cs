using Swashbuckle.AspNetCore.Annotations;
using Expense.Domain.Enums;
using Expense.Domain.Entities;


namespace Expense.Schema.Requests;

public class CreateUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public string IdentityNumber { get; set; }
    public string UserName { get; set; }      
    public string Email { get; set; } 
    public string IBAN { get; set; }
    
    //[SwaggerSchema("User role", Description = "Choose one: Admin or Personnel")]
    public UserRole Role { get; set; }

}

public class UpdateUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public string Email { get; set; } 
    public string IBAN { get; set; }
    public UserRole Role { get; set; }
}


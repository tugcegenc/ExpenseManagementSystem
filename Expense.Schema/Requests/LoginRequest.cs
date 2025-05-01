namespace Expense.Schema.Requests;

public class LoginRequest
{
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }
}

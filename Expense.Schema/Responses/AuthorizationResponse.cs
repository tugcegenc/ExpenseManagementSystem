namespace Expense.Schema.Responses;

public class AuthorizationResponse
{
    public string UserName { get; set; }
    public TokenResponse Token { get; set; }
}

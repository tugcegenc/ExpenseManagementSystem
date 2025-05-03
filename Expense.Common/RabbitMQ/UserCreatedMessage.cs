namespace Expense.Common.RabbitMQ;

public class UserCreatedMessage
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string UserPassword { get; set; } = null!;
}

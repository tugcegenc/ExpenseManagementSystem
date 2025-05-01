namespace Expense.Application.Services.Interfaces;
public interface IAppSession
{
    public long? UserId { get; }
    public string? Email { get; }
    public string? UserName { get; }
    public string? Role { get; }
}

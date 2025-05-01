namespace Expense.Common.Error;

public class ErrorDetail
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public List<string>? Errors { get; set; }
}
using System.Text.Json;

namespace Expense.Common.ApiResponse;

/// <summary>
/// Generic ApiResponse wrapper for data and status
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public DateTime ServerDate { get; set; } = DateTime.UtcNow;
    public Guid ReferenceNo { get; set; } = Guid.NewGuid();

    private ApiResponse() { }

    public static ApiResponse<T> Ok(string message = "Success")
    {
        return new ApiResponse<T> { Success = true, Message = message };
    }

    public static ApiResponse<T> Ok(T data, string? message = null)
    {
        return new ApiResponse<T> { Success = true, Message = message ?? "Success", Data = data };
    }

    public static ApiResponse<T> Fail(string message)
    {
        return new ApiResponse<T> { Success = false, Message = message };
    }

    public static ApiResponse<T> Fail(string message, List<string> errors)
    {
        return new ApiResponse<T> { Success = false, Message = message, Errors = errors };
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public DateTime ServerDate { get; set; } = DateTime.UtcNow;
    public Guid ReferenceNo { get; set; } = Guid.NewGuid();

    private ApiResponse() { }

    public static ApiResponse Ok(string message = "Success")
    {
        return new ApiResponse { Success = true, Message = message };
    }

    public static ApiResponse Fail(string message)
    {
        return new ApiResponse { Success = false, Message = message };
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}
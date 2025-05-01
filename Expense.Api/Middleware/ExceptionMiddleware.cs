using System.Net;
using System.Text.Json;
using Expense.Common.ApiResponse;

namespace Expense.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            var response = ApiResponse<string>.Fail(ex.Message);
            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json); 
        }
    }
}
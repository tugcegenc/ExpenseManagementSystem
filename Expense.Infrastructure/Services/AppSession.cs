using System.Security.Claims;
using Expense.Application.Services.Interfaces.Sessions;
using Microsoft.AspNetCore.Http;

namespace Expense.Infrastructure.Services;

public class AppSession : IAppSession
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AppSession(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public long? UserId
    {
        get
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (long.TryParse(userId, out var id))
            {
                return id;
            }
            return null;
        }
    }

    public string? Email => _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    public string? UserName => _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

    public string? Role => _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
}

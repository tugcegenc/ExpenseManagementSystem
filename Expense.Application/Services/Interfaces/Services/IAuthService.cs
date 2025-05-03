using Expense.Common.ApiResponse;
using Expense.Schema.Requests;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces.Services;

public interface IAuthService
{
    Task<ApiResponse<AuthorizationResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<AuthorizationResponse>> RefreshTokenAsync(string refreshToken);
}

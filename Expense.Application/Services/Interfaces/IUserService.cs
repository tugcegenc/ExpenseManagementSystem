using Expense.Common.ApiResponse;
using Expense.Schema.Requests;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces;

public interface IUserService
{
    Task<ApiResponse<List<UserResponse>>> GetAllAsync();
    Task<ApiResponse<UserResponse>> GetByIdAsync(long id);
    Task<ApiResponse<UserResponse>> CreateAsync(CreateUserRequest request);
    Task<ApiResponse> UpdateAsync(long id, UpdateUserRequest request);
    Task<ApiResponse> DeleteAsync(long id);   
}
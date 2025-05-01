using Expense.Schema.Requests;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces;

public interface IUserService
{
    Task<List<UserResponse>> GetAllAsync();
    Task<UserResponse> GetByIdAsync(long id);
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task UpdateAsync(long id, UpdateUserRequest request);
    Task DeleteAsync(long id);
    
}
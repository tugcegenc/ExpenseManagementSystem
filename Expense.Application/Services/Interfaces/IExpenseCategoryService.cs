using Expense.Common.ApiResponse;
using Expense.Schema.Requests;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces;

public interface IExpenseCategoryService
{
    Task<ApiResponse<List<ExpenseCategoryResponse>>> GetAllAsync();
    Task<ApiResponse<ExpenseCategoryResponse>> GetByIdAsync(long id);
    Task<ApiResponse<ExpenseCategoryResponse>> CreateAsync(CreateExpenseCategoryRequest request);
    Task<ApiResponse> UpdateAsync(long id, UpdateExpenseCategoryRequest request);
    Task<ApiResponse> DeleteAsync(long id);
}

using Expense.Common.ApiResponse;
using Expense.Schema.Requests;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces.Services;

public interface IExpenseCategoryService
{
    Task<ApiResponse<List<ExpenseCategoryResponse>>> GetAllAsync();
    Task<ApiResponse<ExpenseCategoryResponse>> GetByIdAsync(long id);
    Task<ApiResponse<ExpenseCategoryResponse>> CreateAsync(ExpenseCategoryRequest request);
    Task<ApiResponse> UpdateAsync(long id, ExpenseCategoryRequest request);
    Task<ApiResponse> DeleteAsync(long id);
}

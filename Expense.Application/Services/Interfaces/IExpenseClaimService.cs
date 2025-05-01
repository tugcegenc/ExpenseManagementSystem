using Expense.Common.ApiResponse;
using Expense.Schema.Requests;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces;

public interface IExpenseClaimService
{
    Task<ApiResponse<List<ExpenseClaimResponse>>> GetAllAsync();
    Task<ApiResponse<ExpenseClaimResponse>> GetByIdAsync(long id);
    Task<ApiResponse<ExpenseClaimResponse>> CreateAsync(CreateExpenseClaimRequest request);
    Task<ApiResponse> UpdateAsync(long id, UpdateExpenseClaimRequest request);
    Task<ApiResponse> DeleteAsync(long id);
    Task<ApiResponse> ApproveAsync(long id);
    Task<ApiResponse> RejectAsync(long id, string reason);
    Task<ApiResponse<List<ExpenseClaimResponse>>> GetByUserIdAsync();

}
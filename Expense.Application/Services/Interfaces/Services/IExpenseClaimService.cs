using Expense.Common.ApiResponse;
using Expense.Schema.Requests;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces.Services;

public interface IExpenseClaimService
{
    Task<ApiResponse<List<ExpenseClaimResponse>>> GetAllAsync();
    Task<ApiResponse<ExpenseClaimResponse>> GetByIdAsync(long id);
    Task<ApiResponse<ExpenseClaimResponse>> CreateAsync(ExpenseClaimRequest request);
    Task<ApiResponse> UpdateAsync(long id, ExpenseClaimRequest request);
    Task<ApiResponse> DeleteAsync(long id);
    Task<ApiResponse> ApproveAsync(long id);
    Task<ApiResponse> RejectAsync(long id, string reason);
    Task<ApiResponse<List<ExpenseClaimResponse>>> GetClaimsByFilterAsync(ExpenseClaimFilterRequest filter);

}
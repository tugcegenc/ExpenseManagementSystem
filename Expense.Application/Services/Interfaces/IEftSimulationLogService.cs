namespace Expense.Application.Services.Interfaces;

using Expense.Common.ApiResponse;
using Expense.Schema.Requests;
using Expense.Schema.Responses;

public interface IEftSimulationLogService
{
    Task<ApiResponse<List<EftSimulationLogResponse>>> GetAllAsync();
    Task<ApiResponse<EftSimulationLogResponse>> GetByIdAsync(long id);

}

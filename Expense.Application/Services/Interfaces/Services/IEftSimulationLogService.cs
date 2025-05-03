using Expense.Common.ApiResponse;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces.Services;
public interface IEftSimulationLogService
{
    Task<ApiResponse<List<EftSimulationLogResponse>>> GetAllAsync();
    Task<ApiResponse<EftSimulationLogResponse>> GetByIdAsync(long id);

}

using Expense.Common.ApiResponse;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces;

public interface IReportService
{
    Task<ApiResponse<List<PersonnelExpenseResponse>>> GetMyExpensesAsync(long userId);
    Task<ApiResponse<List<ReportSummaryResponse>>> GetDailySummaryAsync();
    Task<ApiResponse<List<ReportSummaryResponse>>> GetWeeklySummaryAsync();
    Task<ApiResponse<List<ReportSummaryResponse>>> GetMonthlySummaryAsync();
    Task<ApiResponse<List<PersonnelSpendingSummaryResponse>>> GetPersonnelDailySpendingAsync();
}

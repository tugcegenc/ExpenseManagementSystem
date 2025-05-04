using Expense.Common.ApiResponse;
using Expense.Domain.Enums;
using Expense.Schema.Reports;

namespace Expense.Application.Services.Interfaces.Services;

public interface IReportService
{
    Task<ApiResponse<List<PersonnelExpenseResponse>>> GetPersonnelExpensesAsync();
    Task<ApiResponse<CompanyExpenseSummaryResponse>> GetCompanyExpenseSummaryAsync(PeriodType periodType);
    Task<ApiResponse<CompanyStatusSummaryResponse>> GetCompanyExpenseStatusSummaryAsync(PeriodType periodType);
    Task<ApiResponse<List<PersonnelExpenseSummaryResponse>>> GetPersonnelExpenseSummaryAsync(PeriodType periodType);
}

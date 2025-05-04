using Expense.Schema.Reports;

namespace Expense.Application.Services.Interfaces.Repositories;

public interface IReportRepository
{
    Task<List<PersonnelExpenseResponse>> GetPersonnelExpensesAsync(long userId);
    Task<CompanyExpenseSummaryResponse> GetCompanyExpenseSummaryAsync(DateTime startDate, DateTime endDate);
    Task<CompanyStatusSummaryResponse> GetCompanyExpenseStatusSummaryAsync(DateTime startDate, DateTime endDate);
    Task<List<PersonnelExpenseSummaryResponse>> GetPersonnelExpenseSummaryAsync(DateTime startDate, DateTime endDate);
    
}


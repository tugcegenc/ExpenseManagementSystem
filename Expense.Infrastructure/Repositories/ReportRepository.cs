using Dapper;
using Expense.Application.Services.Interfaces.Repositories;
using Expense.Infrastructure.Context;
using Expense.Schema.Reports;

namespace Expense.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly DapperContext _context;

    public ReportRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<List<PersonnelExpenseResponse>> GetPersonnelExpensesAsync(long userId)
    {
        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<PersonnelExpenseResponse>(
            "SELECT * FROM dbo.fn_get_user_expenses(@UserId)",
            new { UserId = userId });

        return result.ToList();
    }
    
    public async Task<CompanyExpenseSummaryResponse> GetCompanyExpenseSummaryAsync(DateTime startDate, DateTime endDate)
    {
        using var connection = _context.CreateConnection();

        var sql = "SELECT * FROM dbo.fn_get_company_expense_summary(@StartDate, @EndDate)";
        var parameters = new { StartDate = startDate, EndDate = endDate };

        var result = await connection.QueryFirstOrDefaultAsync<CompanyExpenseSummaryResponse>(sql, parameters);

        result.StartDate = startDate;
        result.EndDate = endDate;
        return result;
    }
    
    public async Task<CompanyStatusSummaryResponse?> GetCompanyExpenseStatusSummaryAsync(DateTime startDate, DateTime endDate)
    {
        var sql = "SELECT * FROM dbo.fn_get_company_expense_status_summary(@StartDate, @EndDate)";
        var parameters = new DynamicParameters();
        parameters.Add("StartDate", startDate);
        parameters.Add("EndDate", endDate);

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<CompanyStatusSummaryResponse>(sql, parameters);
    }
    
    public async Task<List<PersonnelExpenseSummaryResponse>> GetPersonnelExpenseSummaryAsync(DateTime startDate, DateTime endDate)
    {
        var query = "SELECT * FROM dbo.fn_get_personnel_expense_summary(@StartDate, @EndDate)";
        var parameters = new { StartDate = startDate, EndDate = endDate };

        using var connection = _context.CreateConnection();
        var result = await connection.QueryAsync<PersonnelExpenseSummaryResponse>(query, parameters);

        return result.ToList();
    }
}

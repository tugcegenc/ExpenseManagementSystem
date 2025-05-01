using Dapper;
using Expense.Application.Services.Interfaces;
using Expense.Common.ApiResponse;
using Expense.Schema.Responses;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Expense.Application.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IConfiguration _configuration;

    public ReportService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ApiResponse<List<PersonnelExpenseResponse>>> GetMyExpensesAsync(long userId)
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        var sql = @"
            SELECT 
                c.""Name"" AS ""CategoryName"",
                e.""Amount"",
                e.""Status"",
                e.""RequestDate"",
                e.""ApprovedOrRejectedDate"",
                e.""Description""
            FROM ""ExpenseClaims"" e
            JOIN ""ExpenseCategories"" c ON e.""ExpenseCategoryId"" = c.""Id""
            WHERE e.""UserId"" = @UserId AND e.""IsActive"" = true";

        var result = (await connection.QueryAsync<PersonnelExpenseResponse>(sql, new { UserId = userId })).ToList();

        if (!result.Any())
            return ApiResponse<List<PersonnelExpenseResponse>>.Fail("No expense records found for the user.");

        return ApiResponse<List<PersonnelExpenseResponse>>.Ok(result);
    }
    public async Task<List<ReportSummaryResponse>> ExecuteSummaryQueryAsync(string dateCondition)
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        var sql = $@"
            SELECT 
                TO_CHAR(ec.""RequestDate"", 'YYYY-MM-DD') AS ""Date"",
                COUNT(ec.""Id"") AS ""TotalRequests"",
                SUM(CASE WHEN ec.""Status"" = 'Approved' THEN 1 ELSE 0 END) AS ""ApprovedCount"",
                SUM(CASE WHEN ec.""Status"" = 'Rejected' THEN 1 ELSE 0 END) AS ""RejectedCount""
            FROM ""ExpenseClaims"" ec
            WHERE ec.""IsActive"" = true AND {dateCondition}
            GROUP BY TO_CHAR(ec.""RequestDate"", 'YYYY-MM-DD')
            ORDER BY ""Date"" DESC;";

        var result = await connection.QueryAsync<ReportSummaryResponse>(sql);
        return result.ToList();
    }

    public async Task<ApiResponse<List<ReportSummaryResponse>>> GetDailySummaryAsync()
    {
        var data = await ExecuteSummaryQueryAsync("ec.\"RequestDate\" >= CURRENT_DATE");
        if (data == null || data.Count == 0)
            return ApiResponse<List<ReportSummaryResponse>>.Fail("No data found for daily summary");

        return ApiResponse<List<ReportSummaryResponse>>.Ok(data);
    }

    public async Task<ApiResponse<List<ReportSummaryResponse>>> GetWeeklySummaryAsync()
    {
        var data = await ExecuteSummaryQueryAsync("ec.\"RequestDate\" >= CURRENT_DATE - INTERVAL '7 days'");
        if (data == null || data.Count == 0)
            return ApiResponse<List<ReportSummaryResponse>>.Fail("No data found for weekly summary");

        return ApiResponse<List<ReportSummaryResponse>>.Ok(data);
    }

    public async Task<ApiResponse<List<ReportSummaryResponse>>> GetMonthlySummaryAsync()
    {
        var data = await ExecuteSummaryQueryAsync("ec.\"RequestDate\" >= CURRENT_DATE - INTERVAL '30 days'");
        if (data == null || data.Count == 0)
            return ApiResponse<List<ReportSummaryResponse>>.Fail("No data found for monthly summary");

        return ApiResponse<List<ReportSummaryResponse>>.Ok(data);
    }

    public Task<ApiResponse<List<PersonnelSpendingSummaryResponse>>> GetPersonnelDailySpendingAsync()
    {
        throw new NotImplementedException();
    }
}
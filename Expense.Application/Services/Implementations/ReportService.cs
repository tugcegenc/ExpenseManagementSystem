using Expense.Application.Services.Interfaces.Repositories;
using Expense.Application.Services.Interfaces.Services;
using Expense.Application.Services.Interfaces.Sessions;
using Expense.Common.ApiResponse;
using Expense.Domain.Enums;
using Expense.Infrastructure.Helpers;
using Expense.Schema.Reports;

namespace Expense.Application.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IAppSession _appSession;

    public ReportService(IReportRepository reportRepository, IAppSession appSession)
    {
        _reportRepository = reportRepository;
        _appSession = appSession;
    }

    public async Task<ApiResponse<List<PersonnelExpenseResponse>>> GetPersonnelExpensesAsync()
    {
        if (_appSession.UserId is null)
            return ApiResponse<List<PersonnelExpenseResponse>>.Fail("User session not found.");

        var result = await _reportRepository.GetPersonnelExpensesAsync(_appSession.UserId.Value);

        if (result == null || !result.Any())
            return ApiResponse<List<PersonnelExpenseResponse>>.Fail("No expenses found for this user.");

        return ApiResponse<List<PersonnelExpenseResponse>>.Ok(result);
    }

    public async Task<ApiResponse<CompanyExpenseSummaryResponse>> GetCompanyExpenseSummaryAsync(PeriodType periodType)
    {
        var (startDate, endDate) = PeriodHelper.GetPeriodRange(periodType);
        var result = await _reportRepository.GetCompanyExpenseSummaryAsync(startDate, endDate);

        if (result == null)
            return ApiResponse<CompanyExpenseSummaryResponse>.Fail("No data found");

        result.StartDate = startDate;
        result.EndDate = endDate;
        result.PeriodType = periodType.ToString();

        return ApiResponse<CompanyExpenseSummaryResponse>.Ok(result);
    }
    
    public async Task<ApiResponse<CompanyStatusSummaryResponse>> GetCompanyExpenseStatusSummaryAsync(PeriodType periodType)
    {
        var (startDate, endDate) = PeriodHelper.GetPeriodRange(periodType);

        var result = await _reportRepository.GetCompanyExpenseStatusSummaryAsync(startDate, endDate);

        if (result == null)
            return ApiResponse<CompanyStatusSummaryResponse>.Fail("No data found.");

        result.StartDate = startDate;
        result.EndDate = endDate;
        result.PeriodType = periodType.ToString();

        return ApiResponse<CompanyStatusSummaryResponse>.Ok(result);
    }
    public async Task<ApiResponse<List<PersonnelExpenseSummaryResponse>>> GetPersonnelExpenseSummaryAsync(PeriodType periodType)
    {
        var (startDate, endDate) = PeriodHelper.GetPeriodRange(periodType); 

        var result = await _reportRepository.GetPersonnelExpenseSummaryAsync(startDate, endDate);
        if (result == null || result.Count == 0)
            return ApiResponse<List<PersonnelExpenseSummaryResponse>>.Fail("No data found for this period.");
        
        foreach (var item in result)
        {
            item.StartDate = startDate;
            item.EndDate = endDate;
            item.PeriodType = periodType.ToString();
        }
        return ApiResponse<List<PersonnelExpenseSummaryResponse>>.Ok(result);
    }


}



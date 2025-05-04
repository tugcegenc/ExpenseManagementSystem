using Expense.Application.Services.Interfaces.Services;
using Expense.Application.Services.Interfaces.Sessions;
using Expense.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IAppSession _appSession;
    private readonly IReportService _reportService;

    public ReportsController(IAppSession appSession, IReportService reportService)
    {
        _appSession = appSession;
        _reportService = reportService;
    }

    [HttpGet("personnel-expenses")]
    [Authorize(Roles = "Personnel")]
    public async Task<IActionResult> GetPersonnelExpenses()
    {
        var result = await _reportService.GetPersonnelExpensesAsync();
        return Ok(result);
    }

    [HttpGet("company-expense-summary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetCompanyExpenseSummary([FromQuery] PeriodType periodType)
    {
        var result = await _reportService.GetCompanyExpenseSummaryAsync(periodType);
        return Ok(result);
    }
    
    [HttpGet("company-status-summary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetCompanyStatusSummary([FromQuery] PeriodType periodType)
    {
        var result = await _reportService.GetCompanyExpenseStatusSummaryAsync(periodType);
        return Ok(result);
    }
    
    [HttpGet("personnel-expense-summary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPersonnelExpenseSummary([FromQuery] PeriodType periodType)
    {
        var result = await _reportService.GetPersonnelExpenseSummaryAsync(periodType);
        return Ok(result);
    }
}


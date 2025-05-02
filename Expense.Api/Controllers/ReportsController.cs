using Expense.Application.Services.Implementations;
using Expense.Application.Services.Interfaces;
using Expense.Schema.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
     private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("MyExpenses")]
    public async Task<IActionResult> GetMyExpenses([FromQuery] long userId)
    {
        var result = await _reportService.GetMyExpensesAsync(userId);
        return Ok(result);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("DailySummary")]
    public async Task<IActionResult> GetDailySummary()
    {
        var result = await _reportService.GetDailySummaryAsync();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("WeeklySummary")]
    public async Task<IActionResult> GetWeeklySummary()
    {
        var result = await _reportService.GetWeeklySummaryAsync();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("MonthlySummary")]
    public async Task<IActionResult> GetMonthlySummary()
    {
        var result = await _reportService.GetMonthlySummaryAsync();
        return Ok(result);
    }

}
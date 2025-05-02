using Expense.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ExpenseClaimAdminController : ControllerBase
{
    private readonly IExpenseClaimService _expenseClaimService;

    public ExpenseClaimAdminController

(IExpenseClaimService expenseClaimService)
    {
        _expenseClaimService = expenseClaimService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _expenseClaimService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        if (id <= 0)
            return BadRequest("Invalid ID.");
        var result = await _expenseClaimService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("Approve/{id}")]
    public async Task<IActionResult> Approve(long id)
    {
        if (id <= 0)
            return BadRequest("Invalid ID.");
        var result = await _expenseClaimService.ApproveAsync(id);
        return Ok(result);
    }

    [HttpPost("Reject/{id}")]
    public async Task<IActionResult> Reject(long id, [FromQuery] string reason)
    {
        if (id <= 0)
            return BadRequest("Invalid ID.");
        var result = await _expenseClaimService.RejectAsync(id, reason);
        return Ok(result);
    }
}  
using Expense.Application.Services.Interfaces;
using Expense.Schema.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseClaimController : ControllerBase
{
    private readonly IExpenseClaimService _expenseClaimService;

    public ExpenseClaimController(IExpenseClaimService expenseClaimService)
    {
        _expenseClaimService = expenseClaimService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _expenseClaimService.GetAllAsync();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _expenseClaimService.GetByIdAsync(id);
        return Ok(result);
    }

    [Authorize(Roles = "Personnel")]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateExpenseClaimRequest request)   
    {
        var result = await _expenseClaimService.CreateAsync(request);
        return Ok(result);
    }

    [Authorize(Roles = "Personnel")]
    [HttpPut]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateExpenseClaimRequest request)
    {
        var result = await _expenseClaimService.UpdateAsync(id,request);
        return Ok(result);
    }

    [Authorize(Roles = "Personnel")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _expenseClaimService.DeleteAsync(id);
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")] 
    [HttpPost("Approve/{id}")]
    public async Task<IActionResult> Approve(long id)
    {
        var result = await _expenseClaimService.ApproveAsync(id);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Reject/{id}")]
    public async Task<IActionResult> Reject(long id, [FromBody] string reason)
    {
        var result = await _expenseClaimService.RejectAsync(id, reason);
        return Ok(result);
    }
    
    [Authorize(Roles = "Personnel")]
    [HttpGet("MyClaims")]
    public async Task<IActionResult> GetMyExpenseClaims()
    {
        var result = await _expenseClaimService.GetByUserIdAsync();
        return Ok(result);
    }

}  
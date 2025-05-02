using Expense.Application.Services.Interfaces;
using Expense.Schema.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
 [Authorize(Roles = "Personnel")]
public class ExpenseClaimPersonnelController : ControllerBase
{
    private readonly IExpenseClaimService _expenseClaimService;

    public ExpenseClaimPersonnelController(IExpenseClaimService expenseClaimService)
    {
        _expenseClaimService = expenseClaimService;
    }
 
   [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] ExpenseClaimFilterRequest request)
    {
        var result = await _expenseClaimService.GetClaimsByFilterAsync(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] ExpenseClaimRequest request)   
    {
        var result = await _expenseClaimService.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(long id, [FromQuery] ExpenseClaimRequest request)
    {
        if (id <= 0)
            return BadRequest("Invalid ID.");
        var result = await _expenseClaimService.UpdateAsync(id,request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (id <= 0)
            return BadRequest("Invalid ID.");
        var result = await _expenseClaimService.DeleteAsync(id);
        return Ok(result);
    }
}  
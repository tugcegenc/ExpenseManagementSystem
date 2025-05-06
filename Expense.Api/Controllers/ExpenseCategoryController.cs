using Expense.Application.Services.Interfaces.Services;
using Expense.Schema.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseCategoryController : ControllerBase
{
    private readonly IExpenseCategoryService _expenseCategoryService;

    public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService)
    {
        _expenseCategoryService = expenseCategoryService;
    }
    
    [Authorize(Roles = "Admin, Personnel")]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _expenseCategoryService.GetAllAsync();
        return Ok(result);
    }

    [Authorize(Roles = "Admin, Personnel")]
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid ID.");
        var result = await _expenseCategoryService.GetByIdAsync(id);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] ExpenseCategoryRequest request)
    {
        var result = await _expenseCategoryService.CreateAsync(request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Update(long id, [FromQuery] ExpenseCategoryRequest request)
    {
        if (id <= 0)
            return BadRequest("Invalid ID.");
        var result = await _expenseCategoryService.UpdateAsync(id,request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (id <= 0)
            return BadRequest("Invalid ID.");
        var result = await _expenseCategoryService.DeleteAsync(id);
        return Ok(result);
    }
        
}
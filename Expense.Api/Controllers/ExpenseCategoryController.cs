using Expense.Application.Services.Interfaces;
using Expense.Domain.Entities;
using Expense.Schema.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Api.Controllers;
[Authorize(Roles = "Admin")]

[ApiController]
[Route("api/[controller]")]
public class ExpenseCategoryController : ControllerBase
{
    private readonly IExpenseCategoryService _expenseCategoryService;

    public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService)
    {
        _expenseCategoryService = expenseCategoryService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _expenseCategoryService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _expenseCategoryService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExpenseCategoryRequest request)
    {
        var result = await _expenseCategoryService.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateExpenseCategoryRequest request)
    {
        var result = _expenseCategoryService.UpdateAsync(id,request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = _expenseCategoryService.DeleteAsync(id);
        return Ok(result);
    }
        
}
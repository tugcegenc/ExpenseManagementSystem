using Expense.Application.Services.Interfaces.Services;
using Expense.Common.ApiResponse;
using Expense.Schema.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest(ApiResponse.Fail("Invalid ID."));
        var result = await _userService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] CreateUserRequest request)
    {
        var result = await _userService.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(long id, [FromQuery] UpdateUserRequest request)
    {
        if (id <= 0)
            return BadRequest(ApiResponse.Fail("Invalid ID."));
        var result = await _userService.UpdateAsync(id,request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (id <= 0)
            return BadRequest(ApiResponse.Fail("Invalid ID."));
        var result =await _userService.DeleteAsync(id);
        return Ok(result);
    }
}
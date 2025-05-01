using Expense.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Api.Controllers;


[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class EftSimulationLogController : ControllerBase
{
    private readonly IEftSimulationLogService _eftSimulationLogService;

    public EftSimulationLogController(IEftSimulationLogService eftSimulationLogService)
    {
        _eftSimulationLogService = eftSimulationLogService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _eftSimulationLogService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _eftSimulationLogService.GetByIdAsync(id);
        return Ok(result);
    }
}
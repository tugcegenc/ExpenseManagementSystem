using AutoMapper;
using Expense.Application.Services.Interfaces;
using Expense.Application.Services.Interfaces.Infrastucture;
using Expense.Application.Services.Interfaces.Services;
using Expense.Common.ApiResponse;
using Expense.Domain.Entities;
using Expense.Schema.Responses;
using Microsoft.EntityFrameworkCore;

namespace Expense.Application.Services.Implementations;

public class EftSimulationLogService : IEftSimulationLogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EftSimulationLogService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<List<EftSimulationLogResponse>>> GetAllAsync()
    {
        var logs = await _unitOfWork.GetRepository<EftSimulationLog>()
            .AsQueryable()
            .Include(x => x.ExpenseClaim)
            .ThenInclude(c => c.User)
            .Where(x => x.IsActive)
            .ToListAsync();


        var mapped = _mapper.Map<List<EftSimulationLogResponse>>(logs);
        return ApiResponse<List<EftSimulationLogResponse>>.Ok(mapped);
    }


    public async Task<ApiResponse<EftSimulationLogResponse>> GetByIdAsync(long id)
    {
        var log = await _unitOfWork.GetRepository<EftSimulationLog>()
            .AsQueryable()
            .Include(x => x.ExpenseClaim)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        if (log == null)
        {
            return ApiResponse<EftSimulationLogResponse>.Fail($"EFT Simulation Log with ID {id} not found or is inactive.");
        }

        var mapped = _mapper.Map<EftSimulationLogResponse>(log);
        return ApiResponse<EftSimulationLogResponse>.Ok(mapped);
    }
}
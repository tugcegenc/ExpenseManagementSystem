using AutoMapper;
using Expense.Application.Services.Interfaces;
using Expense.Common.ApiResponse;
using Expense.Domain.Entities;
using Expense.Domain.Enums;
using Expense.Domain.Interfaces;
using Expense.Schema.Requests;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Implementations;

public class ExpenseCategoryService : IExpenseCategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAppSession _appSession;
    public ExpenseCategoryService(IUnitOfWork unitOfWork, IMapper mapper, IAppSession appSession)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _appSession = appSession;
    }
    public async Task<ApiResponse<List<ExpenseCategoryResponse>>> GetAllAsync()
    {
        var categories = await _unitOfWork.GetRepository<ExpenseCategory>().GetAllAsync();
        var activeCategories = categories.Where(x => x.IsActive).ToList();

        var mapped = _mapper.Map<List<ExpenseCategoryResponse>>(activeCategories);
        return ApiResponse<List<ExpenseCategoryResponse>>.Ok(mapped);
    }

    public async Task<ApiResponse<ExpenseCategoryResponse>> GetByIdAsync(long id)
    {
        var category = await _unitOfWork.GetRepository<ExpenseCategory>().GetByIdAsync(id);
        if (category == null || !category.IsActive)
        {
            return ApiResponse<ExpenseCategoryResponse>.Fail($"Expense category with ID {id} not found or is inactive.");
        }

        var mapped = _mapper.Map<ExpenseCategoryResponse>(category);
        return ApiResponse<ExpenseCategoryResponse>.Ok(mapped);
    }

    public async Task<ApiResponse<ExpenseCategoryResponse>> CreateAsync(CreateExpenseCategoryRequest request)
    {
        var entity = _mapper.Map<ExpenseCategory>(request);
        entity.CreatedAt = DateTime.UtcNow;
        entity.CreatedBy = _appSession.UserName ?? "Anonymous"; 

        await _unitOfWork.GetRepository<ExpenseCategory>().AddAsync(entity);
        await _unitOfWork.CompleteAsync();

        var response = _mapper.Map<ExpenseCategoryResponse>(entity);
        return ApiResponse<ExpenseCategoryResponse>.Ok(response, "Expense category successfully created.");
    }

    public async Task<ApiResponse> UpdateAsync(long id, UpdateExpenseCategoryRequest request)
    {
        var category = _unitOfWork.GetRepository<ExpenseCategory>();
        var entity = await category.GetByIdAsync(id);

        if (entity == null || !entity.IsActive)
        {
            return ApiResponse.Fail($"Expense category with ID {id} not found or is inactive.");
        }

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = _appSession.UserName ?? "Anonymous"; 

        category.Update(entity);
        await _unitOfWork.CompleteAsync();

        return ApiResponse.Ok("Expense category successfully updated.");
    }

    public async Task<ApiResponse> DeleteAsync(long id)
    {
        var repo = _unitOfWork.GetRepository<ExpenseCategory>();
        var entity = await repo.GetByIdAsync(id);

        if (entity == null || !entity.IsActive)
        {
            return ApiResponse.Fail($"Expense category with ID {id} not found or is already inactive.");
        }

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = _appSession.UserName ?? "Anonymous";

        repo.Update(entity);
        await _unitOfWork.CompleteAsync();

        return ApiResponse.Ok("Expense category successfully deleted.");
    }
}

using AutoMapper;
using Expense.Application.Services.Interfaces;
using Expense.Common.ApiResponse;
using Expense.Domain.Entities;
using Expense.Domain.Enums;
using Expense.Domain.Interfaces;
using Expense.Schema.Requests;
using Expense.Schema.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Expense.Application.Services.Implementations;

public class ExpenseClaimService : IExpenseClaimService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAppSession _appSession;
    private readonly IFileService _fileService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ExpenseClaimService(IUnitOfWork unitOfWork, IMapper mapper, IAppSession appSession, IFileService fileService, IHttpContextAccessor httpContextAccessor)
    {
        _fileService = fileService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _appSession = appSession;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<ApiResponse<List<ExpenseClaimResponse>>> GetAllAsync()
    {
        var claims = _unitOfWork.GetRepository<ExpenseClaim>().AsQueryable().Include(x => x.ExpenseCategory);
        var activeClaims = claims.Where(x => x.IsActive).ToList();

        var mapped = _mapper.Map<List<ExpenseClaimResponse>>(activeClaims);
        return ApiResponse<List<ExpenseClaimResponse>>.Ok(mapped);
    }
    public async Task<ApiResponse<ExpenseClaimResponse>> GetByIdAsync(long id)
    {
        var claim = await _unitOfWork.GetRepository<ExpenseClaim>().AsQueryable()
            .Include(x => x.ExpenseCategory)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        if (claim == null)
        {
            return ApiResponse<ExpenseClaimResponse>.Fail($"Expense claim with ID {id} not found or is inactive.");
        }
        var mapped = _mapper.Map<ExpenseClaimResponse>(claim);
        return ApiResponse<ExpenseClaimResponse>.Ok(mapped);
    }

    public async Task<ApiResponse<ExpenseClaimResponse>> CreateAsync(CreateExpenseClaimRequest request)
    {
        var claim = _mapper.Map<ExpenseClaim>(request);

        claim.UserId = _appSession.UserId ?? throw new Exception("User ID not found in session.");
        claim.CreatedBy = _appSession.UserName ?? "Anonymous";
        claim.Status = ExpenseStatus.Pending;
        claim.RequestDate = DateTime.UtcNow;
        claim.CreatedAt = DateTime.UtcNow;
        claim.IsActive = true;

        if (request.ReceiptFile != null)
        {
            var filePath = await _fileService.SaveFileAsync(request.ReceiptFile);
            claim.ReceiptFilePath = filePath;
        }

        await _unitOfWork.GetRepository<ExpenseClaim>().AddAsync(claim);
        await _unitOfWork.CompleteAsync();

        var insertedClaim = await _unitOfWork.GetRepository<ExpenseClaim>()
            .AsQueryable()
            .Include(x => x.ExpenseCategory)
            .FirstOrDefaultAsync(x => x.Id == claim.Id);

        var response = _mapper.Map<ExpenseClaimResponse>(insertedClaim);

        var requestInfo = _httpContextAccessor.HttpContext?.Request;
        if (requestInfo != null && !string.IsNullOrEmpty(claim.ReceiptFilePath))
        {
            var baseUrl = $"{requestInfo.Scheme}://{requestInfo.Host}";
            response.ReceiptFileUrl = $"{baseUrl}{claim.ReceiptFilePath}";
        }

        return ApiResponse<ExpenseClaimResponse>.Ok(response, "Expense claim successfully created.");
    }
    public async Task<ApiResponse> UpdateAsync(long id, UpdateExpenseClaimRequest request)
    {
        var claim = await _unitOfWork.GetRepository<ExpenseClaim>().GetByIdAsync(id);
        if (claim == null || !claim.IsActive)
            return ApiResponse.Fail($"Expense claim with ID {id} not found or is inactive.");

        if (claim.UserId != _appSession.UserId)
            return ApiResponse.Fail("You are not authorized to access or modify this expense claim.");

        if (claim.Status != ExpenseStatus.Approved || claim.Status != ExpenseStatus.Rejected)
            return ApiResponse.Fail("Requests that have been approved or rejected cannot be updated.");

        var mapped = _mapper.Map(request, claim);

        claim.UpdatedAt = DateTime.UtcNow;
        claim.UpdatedBy = _appSession.UserName ?? "Anonymous";
        claim.IsActive = true;

        _unitOfWork.GetRepository<ExpenseClaim>().Update(mapped);
        await _unitOfWork.CompleteAsync();

        var response = _mapper.Map<ExpenseClaimResponse>(claim);
        return ApiResponse.Ok("Expense claim successfully updated.");
    }
    public async Task<ApiResponse> DeleteAsync(long id)
    {
        var claim = await _unitOfWork.GetRepository<ExpenseClaim>().GetByIdAsync(id);
        if (claim == null || !claim.IsActive)
            return ApiResponse.Fail($"Expense claim with ID {id} not found or is inactive.");

        if (claim.UserId != _appSession.UserId)
            return ApiResponse.Fail("You are not authorized to access or modify this expense claim.");

        if (claim.Status != ExpenseStatus.Approved || claim.Status != ExpenseStatus.Rejected)
            return ApiResponse.Fail("Requests that have been approved or rejected cannot be deleted.");

        claim.IsActive = false;

        _unitOfWork.GetRepository<ExpenseClaim>().Update(claim);
        await _unitOfWork.CompleteAsync();

        return ApiResponse.Ok("Expense claim successfully deleted.");
    }

    public async Task<ApiResponse> ApproveAsync(long id)
    {
        var claim = await _unitOfWork.GetRepository<ExpenseClaim>()
            .AsQueryable()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

        if (claim == null || !claim.IsActive)
        {
            return ApiResponse.Fail($"Expense claim with ID {id} not found or is inactive.");
        }

        if (claim.Status != ExpenseStatus.Pending)
        {
            return ApiResponse.Fail("Only pending claims can be approved.");
        }

        claim.Status = ExpenseStatus.Approved;
        claim.ApprovedOrRejectedDate = DateTime.UtcNow;
        claim.UpdatedAt = DateTime.UtcNow;
        claim.UpdatedBy = _appSession.UserName ?? "Anonymous";

        var eftLog = new EftSimulationLog
        {
            ExpenseClaimId = claim.Id,
            Amount = claim.Amount,
            ReceiverName = $"{claim.User.FirstName} {claim.User.LastName}",
            Description = claim.Description,
            SimulatedAt = DateTime.UtcNow,
            IsSuccessful = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _appSession.UserName ?? "Anonymous",
            IsActive = true
        };
        await _unitOfWork.GetRepository<EftSimulationLog>().AddAsync(eftLog);
        _unitOfWork.GetRepository<ExpenseClaim>().Update(claim);
        await _unitOfWork.CompleteAsync();

        return ApiResponse.Ok("Claim approved and EFT simulation logged.");
    }


    public async Task<ApiResponse> RejectAsync(long id, string reason)
    {
        var claim = await _unitOfWork.GetRepository<ExpenseClaim>().GetByIdAsync(id);
        if (claim == null || !claim.IsActive)
        {
            return ApiResponse.Fail($"Expense claim with ID {id} not found or is inactive.");
        }

        if (claim.Status != ExpenseStatus.Pending)
        {
            return ApiResponse.Fail("Only pending claims can be rejected.");
        }

        claim.Status = ExpenseStatus.Rejected;
        claim.RejectReason = reason;
        claim.ApprovedOrRejectedDate = DateTime.UtcNow;
        claim.UpdatedAt = DateTime.UtcNow;
        claim.UpdatedBy = _appSession.UserName ?? "Anonymous";

        _unitOfWork.GetRepository<ExpenseClaim>().Update(claim);
        await _unitOfWork.CompleteAsync();

        return ApiResponse.Ok("Expense claim rejected.");
    }

    public async Task<ApiResponse<List<ExpenseClaimResponse>>> GetByUserIdAsync()
    {
        var currentUserId = _appSession.UserId;

        var claims = await _unitOfWork.GetRepository<ExpenseClaim>().AsQueryable()
            .Include(x => x.ExpenseCategory)
            .Where(x => x.UserId == currentUserId && x.IsActive)
            .ToListAsync();

        if (claims == null || claims.Count == 0)
            return ApiResponse<List<ExpenseClaimResponse>>.Fail("You have no expense claims.");

        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = request is not null ? $"{request.Scheme}://{request.Host}" : string.Empty;

        var mapped = _mapper.Map<List<ExpenseClaimResponse>>(claims)
            .Select(x =>
            {
                if (!string.IsNullOrEmpty(x.ReceiptFilePath))
                    x.ReceiptFileUrl = baseUrl + x.ReceiptFilePath;
                return x;
            }).ToList();

        return ApiResponse<List<ExpenseClaimResponse>>.Ok(mapped);
    }

}

using AutoMapper;
using Expense.Application.Services.Interfaces;
using Expense.Common.ApiResponse;
using Expense.Common.Helpers;
using Expense.Common.Session;
using Expense.Domain.Entities;
using Expense.Domain.Interfaces;
using Expense.Schema.Requests;
using Expense.Schema.Responses;
using Microsoft.EntityFrameworkCore;

namespace Expense.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAppSession _appSession;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAppSession appSession)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _appSession = appSession;
    }

    public async Task<ApiResponse<List<UserResponse>>> GetAllAsync()
    {
        var users = await _unitOfWork.GetRepository<User>().GetAllAsync();
        var activeUsers = users.Where(x => x.IsActive).ToList();

        var response = _mapper.Map<List<UserResponse>>(activeUsers);
        return ApiResponse<List<UserResponse>>.Ok(response);
    }

    public async Task<ApiResponse<UserResponse>> GetByIdAsync(long id)
    {
        var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
        if (user == null)
            return ApiResponse<UserResponse>.Fail("User not found");
        if (!user.IsActive)
            return ApiResponse<UserResponse>.Fail("User is not active");

        var response = _mapper.Map<UserResponse>(user);
        return ApiResponse<UserResponse>.Ok(response);
    }

    public async Task<ApiResponse<UserResponse>> CreateAsync(CreateUserRequest request)
    {
        var duplicateUser = await _unitOfWork.GetRepository<User>()
            .AsQueryable()
            .FirstOrDefaultAsync(x =>
                x.IsActive && (
                    x.UserName == request.UserName ||
                    x.Email == request.Email ||
                    x.IdentityNumber == request.IdentityNumber ||
                    x.IBAN == request.IBAN));

        if (duplicateUser != null)
        {
            if (duplicateUser.UserName == request.UserName)
                return ApiResponse<UserResponse>.Fail("Username is already taken.");
            if (duplicateUser.Email == request.Email)
                return ApiResponse<UserResponse>.Fail("Email is already registered.");
            if (duplicateUser.IdentityNumber == request.IdentityNumber)
                return ApiResponse<UserResponse>.Fail("Identity number is already in use.");
            if (duplicateUser.IBAN == request.IBAN)
                return ApiResponse<UserResponse>.Fail("IBAN is already in use.");
        }

        var user = _mapper.Map<User>(request);

        var password = PasswordGenerator.GeneratePassword(8);
        var salt = PasswordGenerator.GeneratePassword(30);
        var passwordHash = PasswordGenerator.CreateSHA256(password, salt);
        

        user.PasswordHash = passwordHash;
        user.PasswordSalt = salt;
        user.IsActive = true;
        user.CreatedAt = DateTime.UtcNow;
        user.CreatedBy = _appSession.UserName ?? "Anonymous";

        await _unitOfWork.GetRepository<User>().AddAsync(user);
        await _unitOfWork.CompleteAsync();
        
        var log = $"Username: {user.UserName}, Password: {password}";
        await File.AppendAllTextAsync("GeneratedPasswords.txt", log + Environment.NewLine);

        var response = _mapper.Map<UserResponse>(user);
        return ApiResponse<UserResponse>.Ok(response);
    }

    public async Task<ApiResponse> UpdateAsync(long id, UpdateUserRequest request)
    {
        var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
        if (user == null)
            return ApiResponse.Fail("User not found");

        var duplicateUser = await _unitOfWork.GetRepository<User>()
            .AsQueryable()
            .FirstOrDefaultAsync(x =>
                x.Id != id && x.IsActive && (x.Email == request.Email || x.IBAN == request.IBAN));

        if (duplicateUser != null)
        {
            if (duplicateUser.Email == request.Email)
                return ApiResponse.Fail("Email is already registered.");
            if (duplicateUser.IBAN == request.IBAN)
                return ApiResponse.Fail("IBAN is already in use.");
        }

        _mapper.Map(request, user);
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = _appSession.UserName ?? "Anonymous";

        await _unitOfWork.CompleteAsync();
        return ApiResponse.Ok($"User updated successfully (ID: {id}, Username: {user.FirstName} {user.LastName})");
    }

    public async Task<ApiResponse> DeleteAsync(long id)
    {
        var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
        if (user == null)
            return ApiResponse.Fail("User not found");

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = _appSession.UserName ?? "Anonymous";

        await _unitOfWork.CompleteAsync();
        return ApiResponse.Ok($"User deleted successfully (ID: {id}, Username: {user.FirstName} {user.LastName})");
    }
}

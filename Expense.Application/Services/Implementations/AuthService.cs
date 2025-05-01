using Expense.Application.Services.Interfaces;
using Expense.Common.ApiResponse;
using Expense.Common.Helpers;
using Expense.Domain.Entities;
using Expense.Domain.Interfaces;
using Expense.Schema.Requests;
using Expense.Schema.Responses;
using Microsoft.EntityFrameworkCore;

namespace Expense.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;

    private readonly IUnitOfWork _unitOfWork;

    public AuthService(ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<AuthorizationResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.GetRepository<User>()
            .AsQueryable()
            .FirstOrDefaultAsync(x =>
                (x.Email == request.UserNameOrEmail || x.UserName == request.UserNameOrEmail)
                && x.IsActive);

        if (user == null)
            return ApiResponse<AuthorizationResponse>.Fail("Invalid credentials");

        var passwordHash = PasswordGenerator.CreateSHA256(request.Password, user.PasswordSalt);
        if (user.PasswordHash != passwordHash)
            return ApiResponse<AuthorizationResponse>.Fail("Invalid credentials");

        var accessToken = _tokenService.CreateAccessToken(user);

        var refreshToken = _tokenService.CreateRefreshToken(user);

        await _unitOfWork.GetRepository<RefreshToken>().AddAsync(refreshToken);
        await _unitOfWork.CompleteAsync();

        var response = new AuthorizationResponse
        {
            UserName = user.UserName,
            Token = accessToken
        };

        return ApiResponse<AuthorizationResponse>.Ok(response);
    }

    public async Task<ApiResponse<AuthorizationResponse>> RefreshTokenAsync(string refreshToken)
    {
        var token = await _unitOfWork.GetRepository<RefreshToken>().AsQueryable()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x =>
                x.Token == refreshToken &&
                x.IsActive &&
                x.ExpirationDate > DateTime.UtcNow &&
                !x.IsUsed &&
                !x.IsRevoked);

        if (token == null)
            return ApiResponse<AuthorizationResponse>.Fail("Refresh token not found or expired.");

        token.IsUsed = true;
        token.IsRevoked = true;
        token.IsActive = false;
        token.UpdatedAt = DateTime.UtcNow;
        token.UpdatedBy = token.User.UserName;

        _unitOfWork.GetRepository<RefreshToken>().Update(token);

        var newToken = _tokenService.CreateAccessToken(token.User);
        var newRefreshToken = _tokenService.CreateRefreshToken(token.User);

        await _unitOfWork.GetRepository<RefreshToken>().AddAsync(newRefreshToken);
        await _unitOfWork.CompleteAsync();

        var response = new AuthorizationResponse
        {
            UserName = token.User.UserName,
            Token = newToken,
        };

        return ApiResponse<AuthorizationResponse>.Ok(response);
    }

}
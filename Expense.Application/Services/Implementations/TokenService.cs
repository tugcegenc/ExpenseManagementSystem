using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Expense.Application.Services.Interfaces.Infrastucture;
using Expense.Common.Configurations;
using Expense.Domain.Entities;
using Expense.Schema.Responses;
using Microsoft.IdentityModel.Tokens;

namespace Expense.Application.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly JwtConfig _jwtConfig;

    public TokenService(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }

    public TokenResponse CreateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiration);
        var token = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = CreateRefreshToken(user);

        return new TokenResponse
        {
            AccessToken = accessToken,
            AccessTokenExpiration = expiration,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiration)
        };
    }

    public RefreshToken CreateRefreshToken(User user)
    {
        var tokenBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenBytes);
        }

        var token = Convert.ToBase64String(tokenBytes);

        return new RefreshToken
        {
            Token = token,
            UserId = user.Id,
            ExpirationDate = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiration),
            IsUsed = false,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = user.UserName,
            IsActive = true
        };
    }
}
using Expense.Domain.Entities;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Interfaces.Infrastucture;

public interface ITokenService

{
    TokenResponse CreateAccessToken(User user);
    RefreshToken CreateRefreshToken(User user);
   
}
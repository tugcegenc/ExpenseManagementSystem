using Microsoft.AspNetCore.Http;

namespace Expense.Application.Services.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);
}

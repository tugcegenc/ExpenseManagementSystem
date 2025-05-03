using Microsoft.AspNetCore.Http;

namespace Expense.Application.Services.Interfaces.Services;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);
}

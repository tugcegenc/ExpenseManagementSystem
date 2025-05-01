using Expense.Application.Services.Interfaces;
using Expense.Common.ApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Application.Services.Implementations;

public class FileService : IFileService
{
    private readonly string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return ("File is empty or null.");
        
        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(basePath, fileName);
        Console.WriteLine($"Dosya Yolu: {filePath}");


        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return ($"/uploads/{fileName}");
    }
}
namespace Expense.Application.Services.Interfaces.Services;

public interface IRedisCacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan slidingExpiration, TimeSpan absoluteExpiration);
    Task RemoveAsync(string key);
}
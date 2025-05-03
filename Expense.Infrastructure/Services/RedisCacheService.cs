using System.Text.Json;
using Expense.Application.Services.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Expense.Infrastructure.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _distributedCache;

    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    public async Task<T?> GetAsync<T>(string key)
    {
        var jsonData = await _distributedCache.GetStringAsync(key);

        if(jsonData == null)
            return default;

        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan slidingExpiration, TimeSpan absoluteExpiration)
    {
        var jsonData = JsonSerializer.Serialize(value);

        var options = new DistributedCacheEntryOptions
        {
            SlidingExpiration = slidingExpiration,
            AbsoluteExpiration = DateTime.UtcNow.Add(absoluteExpiration)
        };
        await _distributedCache.SetStringAsync(key, jsonData, options);  
    }
    public async Task RemoveAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}
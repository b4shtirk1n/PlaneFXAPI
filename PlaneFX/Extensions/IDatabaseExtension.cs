using System.Text.Json;
using StackExchange.Redis;

namespace PlaneFX.Extensions
{
    public static class IDatabaseExtension
    {
        public static async Task<T> GetOrSetCacheAsync<T>(this IDatabase redis, string key,
            Func<Task<T>> factory, TimeSpan? expire = null)
        {
            string? cache = await redis.StringGetAsync(key);

            if (!string.IsNullOrEmpty(cache))
                return JsonSerializer.Deserialize<T>(cache)!;

            var data = await factory();
            await redis.StringSetAsync(key, JsonSerializer.Serialize(data), expire);
            return data;
        }
    }
}
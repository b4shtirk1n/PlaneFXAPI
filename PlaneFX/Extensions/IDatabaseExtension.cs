using System.Text.Json;
using StackExchange.Redis;

namespace PlaneFX.Extensions
{
    public static class IDatabaseExtension
    {
        public static async Task<T> GetOrSetCacheAsync<T>(this IDatabase redis, string key,
            Func<Task<T>> factory, TimeSpan? expire = null)
        {
            try
            {
                string? cache = await redis.StringGetAsync(key);

                if (!string.IsNullOrEmpty(cache))
                    return JsonSerializer.Deserialize<T>(cache)!;
            }
            catch
            {
                return await factory();
            }
            var data = await factory();

            var transaction = redis.CreateTransaction();
            await transaction.StringSetAsync(key, JsonSerializer.Serialize(data), expire ?? TimeSpan.FromMinutes(2));
            await transaction.ExecuteAsync();

            return data;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using PlaneFX.Interfaces;
using PlaneFX.Models;
using StackExchange.Redis;

namespace PlaneFX.Services
{
    public class AppService(PlaneFXContext context, IConnectionMultiplexer mux) : IService
    {
        private readonly IDatabase redis = mux.GetDatabase();

        public async Task<string?> GetTickers()
        {
            string key = $"{nameof(AppService)}";
            string? cache = await redis.StringGetAsync(key);

            if (cache == null)
            {
                var res = await context.Services.AsNoTracking()
                    .FirstOrDefaultAsync();

                await redis.StringSetAsync(key, res?.Tickers);
                return res?.Tickers;
            }
            return cache;
        }
    }
}
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
            string? cache = null;
            string key = $"{nameof(AppService)}";

            try
            {
                cache = await redis.StringGetAsync(key);
            }
            catch
            {
            }

            if (cache == null)
            {
                var res = await context.Services.AsNoTracking()
                    .FirstOrDefaultAsync();

                try
                {
                    await redis.StringSetAsync(key, res?.Tickers);
                }
                catch
                {
                }
                return res?.Tickers;
            }
            return cache;
        }
    }
}
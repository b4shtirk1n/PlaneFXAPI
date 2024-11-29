using Microsoft.EntityFrameworkCore;
using PlaneFX.Extensions;
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
            var res = await redis.GetOrSetCacheAsync(nameof(AppService), () => context.Services.AsNoTracking()
                .FirstOrDefaultAsync());

            return res?.Tickers;
        }
    }
}
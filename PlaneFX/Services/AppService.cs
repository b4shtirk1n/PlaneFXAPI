using Microsoft.EntityFrameworkCore;
using PlaneFX.Interfaces;
using PlaneFX.Models;

namespace PlaneFX.Services
{
    public class AppService(PlaneFXContext context) : IService
    {
        public async Task<string?> GetTickers()
        {
            var res = await context.Services.AsNoTracking()
                .FirstOrDefaultAsync();

            return res?.Tickers;
        }
    }
}
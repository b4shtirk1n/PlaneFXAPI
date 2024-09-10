using Microsoft.EntityFrameworkCore;
using PlaneFX.Models;

namespace PlaneFX.Services
{
    public class SubscribeService(PlaneFXContext context)
    {
        public async Task<IEnumerable<Subscription>> Get()
            => await context.Subscriptions.ToListAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using PlaneFX.DTOs;
using PlaneFX.Models;

namespace PlaneFX.Services
{
    public class CommandService(PlaneFXContext context)
    {
        public async Task<IEnumerable<Command>> GetUnComplete()
            => await context.Commands.Where(c => !c.IsComplete).ToListAsync();

        public async Task Create(CommandDTO dTO)
        {
            await context.Commands.AddAsync(new Command
            {
                Account = dTO.Account,
                Order = dTO.Order,
                Volume = dTO.Volume,
                Ticker = dTO.Ticker,
                Price = dTO.Price,
                Type = dTO.Type,
                OrderType = dTO.OrderType,
            });
            await context.SaveChangesAsync();
        }

        public async Task<bool> IsExist(long id)
            => await context.Commands.AnyAsync(c => c.Id == id);

        public async Task Complete(long id)
            => await context.Commands.Where(c => c.Id == id)
                .ExecuteUpdateAsync(u => u.SetProperty(c => c.IsComplete, true));
    }
}
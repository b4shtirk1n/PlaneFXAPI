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
                Type = dTO.Type,
                Ticker = dTO.Ticker,
            });
            await context.SaveChangesAsync();
        }

        public async Task Complete(long id)
        {
            var command = await context.Commands.FindAsync(id);
            command!.IsComplete = true;
            await context.SaveChangesAsync();
        }
    }
}
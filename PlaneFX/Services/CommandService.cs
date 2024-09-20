using Microsoft.EntityFrameworkCore;
using PlaneFX.Models;

namespace PlaneFX.Services
{
    public class CommandService(PlaneFXContext context)
    {
        public async Task<List<Command>> GetUnComplete()
            => await context.Commands.Where(c => !c.IsComplete).ToListAsync();
    }
}
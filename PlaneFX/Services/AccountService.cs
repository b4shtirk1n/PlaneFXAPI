using Microsoft.EntityFrameworkCore;
using PlaneFX.DTOs;
using PlaneFX.Models;

namespace PlaneFX.Services
{
	public class AccountService(PlaneFXContext context)
	{
		public async Task<Account?> GetById(long id)
		  	=> await context.Accounts.FindAsync(id);

		public async Task<Account?> GetByNumber(long number)
		  => await context.Accounts.FirstOrDefaultAsync(a => a.Number == number);

		public async Task<IEnumerable<Account>> GetByUser(long id)
		  	=> await context.Accounts.Where(a => a.User == id).ToListAsync();

		public async Task<bool> IsExist(AccountDTO dTO)
		  	=> await context.Accounts.AnyAsync(a => a.Name == dTO.Name && a.User == dTO.User);

		public async Task<Account> Create(AccountDTO dTO)
		{
			var account = await context.Accounts.AddAsync(new Account
			{
				Name = dTO.Name,
			});
			await context.SaveChangesAsync();
			return account.Entity;
		}
	}
}
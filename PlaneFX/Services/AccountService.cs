using Microsoft.EntityFrameworkCore;
using PlaneFX.DTOs;
using PlaneFX.Models;

namespace PlaneFX.Services
{
	public class AccountService(PlaneFXContext context)
	{
		public async Task<Account?> GetById(long id)
		{
			if (await context.Accounts.FindAsync(id) is not Account account)
				return null;

			account.CountOrders = await context.OpenedOrders.CountAsync(o => o.Account == id);
			return account;
		}

		public async Task<Account?> GetByNumber(long number)
		{
			if (await context.Accounts.FirstOrDefaultAsync(a => a.Number == number)
				is not Account account)
				return null;

			account.CountOrders = await context.OpenedOrders.CountAsync(o => o.Account == account.Id);
			return account;
		}

		public async Task<IEnumerable<Account>> GetByUser(long id)
		{
			var accounts = await context.Accounts.Where(a => a.User == id).ToListAsync();

			foreach (var account in accounts)
				account.CountOrders = await context.OpenedOrders.CountAsync(o => o.Account == account.Id);

			return accounts;
		}

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
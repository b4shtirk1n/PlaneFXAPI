using Microsoft.EntityFrameworkCore;
using PlaneFX.DTOs;
using PlaneFX.Models;
using PlaneFX.Responses;

namespace PlaneFX.Services
{
	public class AccountService(PlaneFXContext context)
	{
		public async Task<AccountResponse?> GetById(long id)
		{
			if (await context.Accounts.FindAsync(id) is not Account account)
				return null;

			int countOrders = await context.OpenedOrders.CountAsync(o => o.Account == id);
			AccountResponse res = (AccountResponse)account;
			res.CountOrders = countOrders;
			return res;
		}

		public async Task<AccountResponse?> GetByNumber(long number)
		{
			if (await context.Accounts.FirstOrDefaultAsync(a => a.Number == number)
				is not Account account)
				return null;

			int countOrders = await context.OpenedOrders.CountAsync(o => o.Account == account.Id);
			AccountResponse res = (AccountResponse)account;
			res.CountOrders = countOrders;
			return res;
		}

		public async Task<IEnumerable<AccountResponse>> GetByUser(long id)
		{
			var accounts = await context.Accounts.Where(a => a.User == id).ToListAsync();
			List<AccountResponse> res = [];

			foreach (var account in accounts)
			{
				int countOrders = await context.OpenedOrders.CountAsync(o => o.Account == account.Id);
				AccountResponse newAccount = (AccountResponse)account;
				newAccount.CountOrders = countOrders;
				res.Add(newAccount);
			}
			return res;
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
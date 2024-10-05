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
			return new(account, countOrders);
		}

		public async Task<AccountResponse?> GetByNumber(long number)
		{
			if (await context.Accounts.FirstOrDefaultAsync(a => a.Number == number)
				is not Account account)
				return null;

			int countOrders = await context.OpenedOrders.CountAsync(o => o.Account == account.Id);
			return new(account, countOrders);
		}

		public async Task<IEnumerable<AccountResponse>> GetByUser(long id)
		{
			var accounts = await context.Accounts.Where(a => a.User == id).ToListAsync();
			List<AccountResponse> res = [];

			foreach (var account in accounts)
			{
				int countOrders = await context.OpenedOrders.CountAsync(o => o.Account == account.Id);
				res.Add(new(account, countOrders));
			}
			return res;
		}

		public async Task<bool> IsExist(AccountDTO dTO)
		  	=> await context.Accounts.AnyAsync(a => a.Name == dTO.Name && a.User == dTO.User);

		public async Task<bool> IsReferral(long user)
			=> await context.UserSubscriptions.AnyAsync(uS => uS.User == user
				&& uS.Date >= DateOnly.FromDateTime(DateTime.Now));

		public async Task<Account> Create(AccountDTO dTO)
		{
			var account = await context.Accounts.AddAsync(new Account
			{
				Name = dTO.Name,
				User = dTO.User,
				Number = dTO.Number,
				IsCent = dTO.IsCent,
			});
			await context.SaveChangesAsync();
			return account.Entity;
		}

		public async Task<Account?> Update(OrderDTO dTO)
		{
			if (await context.Accounts.FirstOrDefaultAsync(a => a.Number == dTO.AccountNumber) is not Account account)
				return null;

			account.Balance = dTO.Balance;
			account.Drawdown = dTO.Drawdown;
			account.MarginLevel = dTO.MarginLevel;
			account.Profit = dTO.Profit;
			account.ProfitToday = dTO.ProfitToday;
			account.ProfitWeek = dTO.ProfitWeek;
			account.ProfitYesterday = dTO.ProfitYesterday;

			await context.SaveChangesAsync();
			return account;
		}
	}
}
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using PlaneFX.DTOs;
using PlaneFX.Interfaces;
using PlaneFX.Models;
using PlaneFX.Responses;

namespace PlaneFX.Services
{
	public class AccountService(PlaneFXContext context, OrderService orderService) : IService
	{
		public async Task<AccountResponse?> GetById(long id)
		{
			if (await context.Accounts.FindAsync(id) is not Account account)
				return null;

			return new(account, await CountOpenOrders(account.Id),
				await orderService.GetProfitOdWeek(account.Id));
		}

		public async Task<AccountResponse?> GetByNumber(long number)
		{
			if (await context.Accounts.AsNoTracking()
				.FirstOrDefaultAsync(a => a.Number == number)
				is not Account account)
				return null;

			return new(account, await CountOpenOrders(account.Id),
				await orderService.GetProfitOdWeek(account.Id));
		}

		public async Task<IEnumerable<AccountResponse>> GetByUser(long id)
		{
			var accounts = await context.Accounts.AsNoTracking()
				.Where(a => a.User == id)
				.ToListAsync();

			List<AccountResponse> res = [];

			foreach (var account in accounts)
			{
				int countOrders = await context.OpenedOrders.AsNoTracking()
					.CountAsync(o => o.Account == account.Id);

				res.Add(new(account, countOrders, await orderService.GetProfitOdWeek(account.Id)));
			}
			return res;
		}

		public async Task<bool> IsExist(AccountDTO dTO)
		  	=> await context.Accounts.AsNoTracking()
				.AnyAsync(a => a.Name == dTO.Name && a.User == dTO.User);

		public async Task<bool> IsReferral(long user)
			=> await context.UserSubscriptions.AsNoTracking()
				.AnyAsync(s => s.User == user && s.Date >= DateOnly.FromDateTime(DateTime.Now));

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
			if (await context.Accounts.FirstOrDefaultAsync(a => a.Number == dTO.AccountNumber)
				is not Account account)
				return null;

			decimal profitability = 1;

			foreach (var order in await orderService.GetCloseOrders(account.Id))
				profitability *= order.Profit / order.PriceOpened + 1;

			account.Balance = dTO.Balance;
			account.Drawdown = dTO.Drawdown;
			account.MarginLevel = dTO.MarginLevel;
			account.Profit = dTO.Profit;
			account.ProfitToday = dTO.ProfitToday;
			account.ProfitWeek = dTO.ProfitWeek;
			account.ProfitYesterday = dTO.ProfitYesterday;
			account.Profitability = (profitability - 1) * 100;

			await context.SaveChangesAsync();
			return account;
		}

		public async Task Rename(long id, string name)
			=> await context.Accounts.Where(a => a.Id == id)
				.ExecuteUpdateAsync(u => u.SetProperty(a => a.Name, name));

		public async Task Remove(long number)
		{
			if (await context.Accounts.FirstOrDefaultAsync(a => a.Number == number)
				is not Account account)
				return;

			context.Accounts.Remove(account);
			await context.SaveChangesAsync();
			return;
		}

		private async Task<int> CountOpenOrders(long id)
			=> await context.OpenedOrders.AsNoTracking()
				.CountAsync(o => o.Account == id);
	}
}
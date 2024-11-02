using Microsoft.EntityFrameworkCore;
using PlaneFX.DTOs;
using PlaneFX.Models;
using PlaneFX.Responses;

namespace PlaneFX.Services
{
    public class OrderService(PlaneFXContext context)
    {
        private const int TAKE = 10;

        public async Task<OrderResponse> Get(long id)
            => new(await GetOpenOrders(id), await GetCloseOrders(id));

        public async Task<PaginationResponse<OpenedOrder>> GetOpenOrders(long id, int page = 1)
            => await Pagination(context.OpenedOrders.AsNoTracking()
                .Where(o => o.Account == id), page);

        public async Task<PaginationResponse<ClosedOrder>> GetCloseOrders(long id, int page = 1)
            => await Pagination(context.ClosedOrders.AsNoTracking()
                .Where(o => o.Account == id), page);

        public async Task Process(OrderDTO dTO, long accountId)
        {
            await Clear(accountId);

            foreach (var openOrderDTO in dTO.OpenedOrders)
                await CreateOpen(openOrderDTO, accountId, dTO.Timestamp);

            List<ClosedOrderDTO> unExistedOrders = dTO.ClosedOrders
                .Where(o => !context.ClosedOrders
                    .AsNoTracking()
                    .Where(c => c.Account == accountId)
                    .Select(c => c.Order)
                    .Contains(o.Order))
                .ToList();

            foreach (var closeOrderDTO in unExistedOrders)
                await CreateClose(closeOrderDTO, accountId);

            await context.SaveChangesAsync();
        }

        private async Task<PaginationResponse<T>> Pagination<T>(IQueryable<T> query, int page)
        {
            IEnumerable<T> orders = await query
                .Skip(TAKE * (page - 1))
                .Take(TAKE + 1)
                .ToListAsync();

            T lastOrder = orders.Last();
            orders = orders.Take(TAKE);

            return new(orders, orders.Last()!.Equals(lastOrder));
        }

        private async Task CreateOpen(OpenedOrderDTO dTO, long accountId, long timeUpdate)
            => await context.OpenedOrders.AddAsync(new()
            {
                Account = accountId,
                Order = dTO.Order,
                Volume = dTO.Volume,
                TimeOpened = DateTimeOffset.FromUnixTimeSeconds(dTO.TimeOpened).UtcDateTime,
                PriceOpened = dTO.PriceOpened,
                Sl = dTO.Sl,
                Tp = dTO.Tp,
                Swap = dTO.Swap,
                Commissions = dTO.Commissions,
                Profit = dTO.Profit,
                Symbol = dTO.Symbol,
                TimeUpdate = DateTimeOffset.FromUnixTimeSeconds(timeUpdate).UtcDateTime,
            });

        private async Task CreateClose(ClosedOrderDTO dTO, long accountId)
            => await context.ClosedOrders.AddAsync(new()
            {
                Account = accountId,
                Order = dTO.Order,
                Volume = dTO.Volume,
                TimeOpened = DateTimeOffset.FromUnixTimeSeconds(dTO.TimeClosed).UtcDateTime,
                TimeClosed = DateTimeOffset.FromUnixTimeSeconds(dTO.TimeClosed).UtcDateTime,
                PriceOpened = dTO.PriceOpened,
                Sl = dTO.Sl,
                Tp = dTO.Tp,
                Swap = dTO.Swap,
                Commissions = dTO.Commissions,
                Profit = dTO.Profit,
                Symbol = dTO.Symbol,
            });

        private async Task Clear(long accountId)
            => await context.OpenedOrders.AsNoTracking()
                .Where(o => o.Account == accountId)
                .ExecuteDeleteAsync();
    }
}
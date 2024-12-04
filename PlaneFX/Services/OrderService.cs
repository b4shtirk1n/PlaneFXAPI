using Microsoft.EntityFrameworkCore;
using PlaneFX.DTOs;
using PlaneFX.Extensions;
using PlaneFX.Interfaces;
using PlaneFX.Models;
using PlaneFX.Responses;
using StackExchange.Redis;

namespace PlaneFX.Services
{
    public class OrderService(PlaneFXContext context, IConnectionMultiplexer mux) : IService
    {
        private const int TAKE = 10;
        private readonly IDatabase redis = mux.GetDatabase();

        public async Task<OrderResponse> Get(long accountId)
            => new(await GetOpenOrders(accountId), await GetCloseOrders(accountId));

        public async Task<OrderResponseV2> GetV2(long accountId)
            => new(await GetOpenOrdersV2(accountId), await GetCloseOrdersV2(accountId));

        public async Task<IEnumerable<OpenedOrder>> GetOpenOrders(long accountId)
            => await context.OpenedOrders.AsNoTracking()
                .Where(o => o.Account == accountId)
                .ToListAsync();

        public async Task<IEnumerable<ClosedOrder>> GetCloseOrders(long accountId)
            => await redis.GetOrSetCacheAsync($"{nameof(Order)}:{accountId}", ()
                => context.ClosedOrders.AsNoTracking()
                    .Where(o => o.Account == accountId)
                    .OrderByDescending(o => o.TimeClosed)
                    .ToListAsync());

        public async Task<decimal> GetProfitOdWeek(long accountId)
            => await redis.GetOrSetCacheAsync($"{nameof(DateTimeExtensions.StartOfWeek)}:{accountId}", async () =>
                {
                    decimal profitOfWeek = 0;
                    DateTime startOfWeek = DateTime.Now
                        .StartOfWeek(DayOfWeek.Monday)
                        .ToUniversalTime();

                    var orders = await context.ClosedOrders.AsNoTracking()
                        .Where(o => o.Account == accountId && o.TimeClosed >= startOfWeek)
                        .ToListAsync();

                    foreach (var order in orders)
                        profitOfWeek += order.Profit + order.Swap + order.Commissions;

                    return profitOfWeek;
                });

        public async Task<PaginationResponse<OpenedOrder>> GetOpenOrdersV2(long accountId, int page = 1)
            => await context.OpenedOrders.AsNoTracking()
                .Where(o => o.Account == accountId)
                .Pagination(TAKE, page);

        public async Task<PaginationResponse<ClosedOrder>> GetCloseOrdersV2(long accountId, int page = 1)
            => await context.ClosedOrders.AsNoTracking()
                .Where(o => o.Account == accountId)
                .OrderByDescending(o => o.TimeClosed)
                .Pagination(TAKE, page);

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

            if (unExistedOrders.Count > 0)
                await redis.KeyDeleteAsync([$"{nameof(Order)}:{accountId}",
                    $"{nameof(DateTimeExtensions.StartOfWeek)}:{accountId}"]);
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
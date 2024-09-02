using Microsoft.EntityFrameworkCore;
using PlaneFX.DTOs;
using PlaneFX.Models;
using PlaneFX.Responses;

namespace PlaneFX.Services
{
    public class OrderService(PlaneFXContext context)
    {
        public async Task<OrderResponse> GetOrders(long account)
            => new(await GetOpenOrders(account), await GetCloseOrders(account));

        public async Task<IEnumerable<OpenedOrder>> GetOpenOrders(long account)
            => await context.OpenedOrders.Where(o => o.Account == account).ToListAsync();

        public async Task<IEnumerable<ClosedOrder>> GetCloseOrders(long account)
            => await context.ClosedOrders.Where(o => o.Account == account).ToListAsync();

        public async Task<bool> OrderExist(long order)
            => await context.OpenedOrders.AnyAsync(o => o.Order == order)
                || await context.ClosedOrders.AnyAsync(o => o.Order == order);

        public async Task Process(OrderDTO dTO, long account)
        {
            foreach (var openOrderDTO in dTO.OpenedOrders)
                if (await context.OpenedOrders.FirstOrDefaultAsync(o => o.Order == openOrderDTO.Order)
                    is OpenedOrder openOrder)
                    Update(openOrder, openOrderDTO);
                else
                    await CreateOpen(openOrderDTO, account);

            foreach (var closeOrderDTO in dTO.ClosedOrders)
                if (await context.OpenedOrders.FirstOrDefaultAsync(o => o.Order == closeOrderDTO.Order)
                    is OpenedOrder openOrder)
                {
                    Close(openOrder);
                    await CreateClose(closeOrderDTO, account);
                }
                else
                    await CreateClose(closeOrderDTO, account);

            await context.SaveChangesAsync();
        }

        private async Task CreateOpen(OpenedOrderDTO dTO, long account)
            => await context.OpenedOrders.AddAsync(new()
            {
                Account = account,
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
                TimeUpdate = DateTimeOffset.FromUnixTimeSeconds(dTO.TimeUpdate).UtcDateTime,
            });

        private async Task CreateClose(ClosedOrderDTO dTO, long account)
            => await context.ClosedOrders.AddAsync(new()
            {
                Account = account,
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

        private static void Update(OpenedOrder openOrder, OpenedOrderDTO dTO)
        {
            openOrder.Volume = dTO.Volume;
            openOrder.TimeOpened = DateTimeOffset.FromUnixTimeSeconds(dTO.TimeOpened).UtcDateTime;
            openOrder.PriceOpened = dTO.PriceOpened;
            openOrder.Sl = dTO.Sl;
            openOrder.Tp = dTO.Tp;
            openOrder.Swap = dTO.Swap;
            openOrder.Commissions = dTO.Commissions;
            openOrder.Profit = dTO.Profit;
            openOrder.Symbol = dTO.Symbol;
            openOrder.TimeUpdate = DateTimeOffset.FromUnixTimeSeconds(dTO.TimeOpened).UtcDateTime;
        }

        private void Close(OpenedOrder openOrder)
            => context.OpenedOrders.Remove(openOrder);
    }
}
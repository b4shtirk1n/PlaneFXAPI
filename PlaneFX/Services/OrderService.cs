using Microsoft.EntityFrameworkCore;
using PlaneFX.DTOs;
using PlaneFX.Models;
using PlaneFX.Responses;

namespace PlaneFX.Services
{
    public class OrderService(PlaneFXContext context)
    {
        public async Task<OrderResponse> Get(long id)
            => new(await GetOpenOrders(id), await GetCloseOrders(id));

        public async Task<IEnumerable<OpenedOrder>> GetOpenOrders(long id)
            => await context.OpenedOrders.Where(o => o.Account == id).ToListAsync();

        public async Task<IEnumerable<ClosedOrder>> GetCloseOrders(long id)
            => await context.ClosedOrders.Where(o => o.Account == id).ToListAsync();

        public async Task<IEnumerable<OrderType>> GetTypes()
            => await context.OrderTypes.ToListAsync();

        public async Task<bool> OrderExist(long orderId)
            => await context.OpenedOrders.AnyAsync(o => o.Order == orderId)
                || await context.ClosedOrders.AnyAsync(o => o.Order == orderId);

        public async Task Process(OrderDTO dTO, long accountId)
        {
            foreach (var openOrderDTO in dTO.OpenedOrders)
                if (await context.OpenedOrders.FirstOrDefaultAsync(o => o.Order == openOrderDTO.Order)
                    is OpenedOrder openOrder)
                    Update(openOrder, openOrderDTO, dTO.Timestamp);
                else
                    await CreateOpen(openOrderDTO, accountId, dTO.Timestamp);

            foreach (var closeOrderDTO in dTO.ClosedOrders)
                if (await context.OpenedOrders.FirstOrDefaultAsync(o => o.Order == closeOrderDTO.Order)
                    is OpenedOrder openOrder)
                {
                    Close(openOrder);
                    await CreateClose(closeOrderDTO, accountId);
                }
                else
                    await CreateClose(closeOrderDTO, accountId);

            await context.SaveChangesAsync();
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

        private static void Update(OpenedOrder openOrder, OpenedOrderDTO dTO, long timeUpdate)
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
            openOrder.TimeUpdate = DateTimeOffset.FromUnixTimeSeconds(timeUpdate).UtcDateTime;
        }

        private void Close(OpenedOrder openOrder)
            => context.OpenedOrders.Remove(openOrder);
    }
}
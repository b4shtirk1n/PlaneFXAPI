using PlaneFX.Models;

namespace PlaneFX.Responses
{
    public class OrderResponse(IEnumerable<OpenedOrder> paginationOpenedOrders,
        IEnumerable<ClosedOrder> paginationClosedOrders)
    {
        public IEnumerable<OpenedOrder> PaginationOpenedOrders { get; set; } = paginationOpenedOrders;

        public IEnumerable<ClosedOrder> PaginationClosedOrders { get; set; } = paginationClosedOrders;
    }
}
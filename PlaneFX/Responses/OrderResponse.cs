using PlaneFX.Models;

namespace PlaneFX.Responses
{
    public class OrderResponse(IEnumerable<OpenedOrder> openedOrders, IEnumerable<ClosedOrder> closedOrders)
    {
        public IEnumerable<OpenedOrder> OpenedOrders { get; set; } = openedOrders;

        public IEnumerable<ClosedOrder> ClosedOrders { get; set; } = closedOrders;
    }
}
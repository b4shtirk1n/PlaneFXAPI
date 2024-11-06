using PlaneFX.Models;

namespace PlaneFX.Responses
{
    public class OrderResponse(IEnumerable<OpenedOrder> OpenedOrders, IEnumerable<ClosedOrder> ClosedOrders)
    {
        public IEnumerable<OpenedOrder> OpenedOrders { get; set; } = OpenedOrders;

        public IEnumerable<ClosedOrder> ClosedOrders { get; set; } = ClosedOrders;
    }
}
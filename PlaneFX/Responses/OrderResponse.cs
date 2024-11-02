using PlaneFX.Models;

namespace PlaneFX.Responses
{
    public class OrderResponse(PaginationResponse<OpenedOrder> openedOrders,
        PaginationResponse<ClosedOrder> closedOrders)
    {
        public PaginationResponse<OpenedOrder> OpenedOrders { get; set; } = openedOrders;

        public PaginationResponse<ClosedOrder> ClosedOrders { get; set; } = closedOrders;
    }
}
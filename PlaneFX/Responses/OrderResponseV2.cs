using PlaneFX.Models;

namespace PlaneFX.Responses
{
    public class OrderResponseV2(PaginationResponse<OpenedOrder> paginationOpenedOrders,
        PaginationResponse<ClosedOrder> paginationClosedOrders)
    {
        public PaginationResponse<OpenedOrder> PaginationOpenedOrders { get; set; } = paginationOpenedOrders;

        public PaginationResponse<ClosedOrder> PaginationClosedOrders { get; set; } = paginationClosedOrders;
    }
}
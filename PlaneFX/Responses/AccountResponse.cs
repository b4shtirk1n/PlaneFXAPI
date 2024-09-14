using PlaneFX.Models;

namespace PlaneFX.Responses
{
    public class AccountResponse(int countOrders) : Account
    {
        public int CountOrders { get; set; } = countOrders;
    }
}
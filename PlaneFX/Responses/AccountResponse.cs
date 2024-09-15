using PlaneFX.Models;

namespace PlaneFX.Responses
{
    public class AccountResponse(Account account, int countOrders)
    {
        public Account Account { get; set; } = account;

        public int CountOrders { get; set; } = countOrders;
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlaneFX.Responses;

namespace PlaneFX.Extensions
{
    public static class QueryableExtension
    {
        public static async Task<PaginationResponse<T>> Pagination<T>(this IQueryable<T> query, int take, int page)
        {
            IEnumerable<T> orders = await query
                .Skip(take * (page - 1))
                .Take(take + 1)
                .ToListAsync();

            if (orders.IsNullOrEmpty())
                return new(orders);

            T lastOrder = orders.Last();
            orders = orders.Take(take);

            return new(orders, !orders.Last()!.Equals(lastOrder));
        }
    }
}
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PlaneFX.Models;

namespace PlaneFX.Services
{
    public class SubscribeService(IConfiguration configuration, PlaneFXContext context)
    {
        public async Task<IEnumerable<Subscription>> Get()
            => await context.Subscriptions.ToListAsync();

        public async Task<string> CreateInvoice(Invoice invoice)
        {
            using HttpClient client = new();
            string json = JsonSerializer.Serialize(invoice);
            var content = new StringContent(JsonSerializer.Serialize(invoice),
                Encoding.UTF8, "application/json");

            string token = configuration[StartupService.TG_API_TOKEN]!;
            var response = await client.PostAsync($"https://api.telegram.org/{token}/createInvoiceLink",
                content);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using PlaneFX.Interfaces;
using PlaneFX.Models;
using PlaneFX.Requests;
using Telegram.Bot;

namespace PlaneFX.Services
{
    public class SubscribeService(IConfiguration configuration, PlaneFXContext context) : IService
    {
        public async Task<IEnumerable<Subscription>> Get()
            => await context.Subscriptions.ToListAsync();

        public async Task<string> CreateInvoice(InvoiceRequest invoice)
        {
            var client = new TelegramBotClient(configuration[StartupService.TG_API_TOKEN]!);

            return await client.CreateInvoiceLink(invoice.Title, invoice.Description,
                invoice.Payload, invoice.Currency, [(invoice.Prices[0].Label, invoice.Prices[0].Amount)]);
        }
    }
}
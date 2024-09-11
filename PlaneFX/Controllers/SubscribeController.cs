using Microsoft.AspNetCore.Mvc;
using PlaneFX.Models;
using PlaneFX.Services;

namespace PlaneFX.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SubscribeController(SubscribeService subscribeService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> Get()
            => Ok(await subscribeService.Get());

        [HttpPost("Checkout")]
        public async Task<ActionResult<string>> CreateInvoice(Invoice invoice)
            => Ok(await subscribeService.CreateInvoice(invoice));
    }
}
using Microsoft.AspNetCore.Mvc;
using PlaneFX.Models;
using PlaneFX.Services;

namespace PlaneFX.Controllers
{
    public class SubscribeController(SubscribeService subscribeService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> Get()
            => Ok(await subscribeService.Get());
    }
}
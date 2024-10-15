using Microsoft.AspNetCore.Mvc;
using PlaneFX.Services;

namespace PlaneFX.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ServiceController(AppService appService) : ControllerBase
    {
        [HttpGet("GetTickers")]
        public async Task<ActionResult<string>> GetTickers()
            => Ok(await appService.GetTickers());
    }
}
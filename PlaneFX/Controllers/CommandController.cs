using Microsoft.AspNetCore.Mvc;
using PlaneFX.DTOs;
using PlaneFX.Services;

namespace PlaneFX.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CommandController(CommandService commandService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CommandDTO dTO)
        {
            await commandService.Create(dTO);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Complete(long id)
        {
            await commandService.Complete(id);
            return Ok();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using PlaneFX.DTOs;
using PlaneFX.Requests;
using PlaneFX.Services;

namespace PlaneFX.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CommandController(UserService userService, CommandService commandService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CommandDTO dTO)
        {
            await commandService.Create(dTO);
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Complete(CommandRequest request)
        {
            if (await userService.GetByToken(request.Token) == null)
                return Unauthorized();

            if (!await commandService.IsExist(request.Id))
                return NotFound();

            await commandService.Complete(request.Id);
            return Ok();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using PlaneFX.DTOs;
using PlaneFX.Requests;
using PlaneFX.Services;

namespace PlaneFX.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CommandController(
        UserService userService,
        AccountService accountService,
        CommandService commandService
    ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CommandDTO dTO)
        {
            await commandService.Create(dTO);
            return Ok();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Complete(long id, CommandRequest request)
        {
            if (await userService.GetByToken(request.Token) == null)
                return Unauthorized();

            if (!await commandService.IsExist(id)
                || await accountService.GetByNumber(request.AccountNumber) == null)
                return NotFound();

            await commandService.Complete(id);
            return Ok();
        }
    }
}
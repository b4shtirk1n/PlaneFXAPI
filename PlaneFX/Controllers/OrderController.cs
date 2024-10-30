using Microsoft.AspNetCore.Mvc;
using PlaneFX.DTOs;
using PlaneFX.Models;
using PlaneFX.Responses;
using PlaneFX.Services;

namespace PlaneFX.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrderController(
        OrderService orderService,
        AccountService accountService,
        UserService userService,
        CommandService commandService,
        ILogger<OrderController> logger
    ) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> Get(long id)
            => Ok(await orderService.Get(id));

        [HttpGet("GetOpen/{id}")]
        public async Task<ActionResult<List<OpenedOrder>>> GetOpened(long id)
            => Ok(await orderService.GetOpenOrders(id));

        [HttpGet("GetClose/{id}")]
        public async Task<ActionResult<List<OpenedOrder>>> GetClosed(long id)
            => Ok(await orderService.GetCloseOrders(id));

        [HttpPost]
        public async Task<ActionResult<List<Command>>> Update(OrderDTO dTO)
        {
            if (await accountService.GetByNumber(dTO.AccountNumber) is not AccountResponse account)
            {
                if (await userService.GetByToken(dTO.Token) is not User user)
                    return BadRequest();

                await accountService.Create(new AccountDTO()
                {
                    User = user.Id,
                    Name = dTO.AccountName,
                    Number = dTO.AccountNumber,
                    IsCent = dTO.IsCent,
                });

                account = await accountService.GetByNumber(dTO.AccountNumber!)
                    ?? throw new NullReferenceException();
            }
            try
            {
                await accountService.Update(dTO);
                await orderService.Process(dTO, account.Account.Id);

                return Ok(await commandService.GetUnComplete());
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public async Task<ActionResult<IEnumerable<OpenedOrder>>> GetOpened(long id)
            => Ok(await orderService.GetOpenOrders(id));

        [HttpGet("GetClose/{id}")]
        public async Task<ActionResult<IEnumerable<ClosedOrder>>> GetClosed(long id)
            => Ok(await orderService.GetCloseOrders(id));

        [HttpGet("V2/GetOpen/{id}")]
        public async Task<ActionResult<PaginationResponse<OpenedOrder>>> GetOpenedV2(long id)
            => Ok(await orderService.GetOpenOrdersV2(id));

        [HttpGet("V2/GetClose/{id}")]
        public async Task<ActionResult<PaginationResponse<ClosedOrder>>> GetClosedV2(long id)
            => Ok(await orderService.GetCloseOrdersV2(id));

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
            catch (Exception ex)
            {
                var DtoString = JsonSerializer.Serialize(dTO);
                logger.LogInformation(DtoString);
                logger.LogError(ex.Message);
                return Conflict();
            }
        }
    }
}
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
    public class OrderController(OrderService orderService, AccountService accountService) : ControllerBase
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
        public async Task<IActionResult> Update(OrderDTO dTO)
        {
            if (await accountService.GetByNumber(dTO.AccountNumber) is not AccountResponse account)
                return BadRequest();

            try
            {
                await accountService.Update(dTO);
                await orderService.Process(dTO, account.Account.Id);
                return Ok();
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
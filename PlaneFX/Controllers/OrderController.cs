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
        [HttpGet]
        public async Task<ActionResult<OrderResponse>> Get(long account)
            => Ok(await orderService.GetOrders(account));

        [HttpGet("GetOpen/{account}")]
        public async Task<ActionResult<List<OpenedOrder>>> GetOpened(long account)
            => Ok(await orderService.GetOpenOrders(account));

        [HttpGet("GetClose/{account}")]
        public async Task<ActionResult<List<OpenedOrder>>> GetClosed(long account)
            => Ok(await orderService.GetCloseOrders(account));

        [HttpPost]
        public async Task<IActionResult> Update(OrderDTO dTO)
        {
            if (await accountService.GetByNumber(dTO.AccountNumber) is not Account account)
                return BadRequest();

            try
            {
                await orderService.Process(dTO, account.Id);
                return Ok();
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
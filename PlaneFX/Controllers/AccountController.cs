using Microsoft.AspNetCore.Mvc;
using PlaneFX.DTOs;
using PlaneFX.Responses;
using PlaneFX.Services;

namespace PlaneFX.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class AccountController(AccountService accountService) : ControllerBase
	{
		[HttpGet("{id}")]
		public async Task<ActionResult<AccountResponse>> GetById(long id)
			=> await accountService.GetById(id) is AccountResponse account
				? Ok(account)
				: NotFound();

		[HttpGet("User/{id}")]
		public async Task<ActionResult<IEnumerable<AccountResponse>>> GetByUser(long id)
			=> Ok(await accountService.GetByUser(id));

		[HttpPost]
		public async Task<ActionResult<AccountResponse>> Create(AccountDTO dTO)
		  	=> await accountService.IsExist(dTO)
				? Ok(await accountService.Create(dTO))
				: Conflict();
	}
}
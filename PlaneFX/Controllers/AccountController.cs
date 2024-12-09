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

		[HttpGet]
		public async Task<ActionResult<bool>> IsReferral(long id)
			=> Ok(await accountService.IsReferral(id));

		[HttpPost]
		public async Task<ActionResult<AccountResponse>> Create(AccountDTO dTO)
		  	=> await accountService.IsExist(dTO)
				? Ok(await accountService.Create(dTO))
				: Conflict();

		[HttpPatch]
		public async Task<IActionResult> Rename(long id, string name)
		{
			await accountService.Rename(id, name);
			return Ok();
		}

		[HttpDelete("{number}")]
		public async Task<IActionResult> Delete(long number)
		{
			if (await accountService.GetByNumber(number) != null)
				await accountService.Remove(number);

			return Ok();
		}
	}
}
using Microsoft.AspNetCore.Mvc;
using PlaneFX.DTOs;
using PlaneFX.Models;
using PlaneFX.Services;

namespace PlaneFX.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class AccountController(AccountService accountService) : ControllerBase
	{
		[HttpGet("{id}")]
		public async Task<ActionResult<Account>> GetById(long id)
			=> await accountService.GetById(id) is Account account
				? Ok(account)
				: NotFound();

		[HttpGet("User/{id}")]
		public async Task<ActionResult<IEnumerable<Account>>> GetByUser(long id)
			=> Ok(await accountService.GetByUser(id));

		[HttpPost]
		public async Task<ActionResult<Account>> Create(AccountDTO dTO)
		  	=> await accountService.IsExist(dTO)
				? Ok(await accountService.Create(dTO))
				: Conflict();
	}
}
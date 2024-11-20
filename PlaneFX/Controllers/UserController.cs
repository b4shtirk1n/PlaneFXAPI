using System.Web;
using Microsoft.AspNetCore.Mvc;
using PlaneFX.DTOs;
using PlaneFX.Enums;
using PlaneFX.Filters;
using PlaneFX.Models;
using PlaneFX.Services;

namespace PlaneFX.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class UserController(UserService userService) : ControllerBase
	{
		[HttpGet]
		[ServiceFilter<AuthFilter>]
		public async Task<ActionResult<IEnumerable<User>>> Get()
		{
			User? user = await userService.GetByToken(Request.Headers.Authorization!);

			return user!.Role switch
			{
				(int)RoleEnum.SU => Ok(await userService.Get()),
				(int)RoleEnum.Admin => Ok(await userService.GetAllFromAdmin()),
				_ => BadRequest(),
			};
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<User>> Get(long id)
			=> await userService.GetById(id) is User user ? Ok(user) : NotFound();

		[HttpGet("Photo/{url}")]
		public IActionResult GetPhoto(string url)
			=> Redirect(HttpUtility.UrlDecode(url));

		[HttpPost]
		public async Task<ActionResult<User>> SignIn(UserDTO dTO)
			=> await userService.GetByTg(dTO.TgId) is User user
				? Ok(await userService.ChangeUsername(user, dTO.Username))
				: Ok(await userService.Add(dTO));
	}
}
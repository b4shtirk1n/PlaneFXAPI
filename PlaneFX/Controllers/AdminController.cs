using Microsoft.AspNetCore.Mvc;
using PlaneFX.Enums;
using PlaneFX.Filters;
using PlaneFX.Models;
using PlaneFX.Services;

namespace PlaneFX.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	[ServiceFilter<AuthFilter>]
	[ServiceFilter<AdminFilter>]
	public class AdminController(
		UserService userService,
		AdminService adminService
	) : ControllerBase
	{
		[HttpPatch("{id}")]
		public async Task<IActionResult> MakeAdmin(int id)
		{
			if (await userService.GetById(id) is not User user)
				return NotFound();

			if (user.Role >= (int)RoleEnum.Admin)
				return Conflict();

			await adminService.MakeAdmin(user);
			return Ok();
		}
	}
}
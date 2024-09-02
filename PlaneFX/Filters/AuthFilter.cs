using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PlaneFX.Services;

namespace PlaneFX.Filters
{
	public class AuthFilter(UserService userService) : IAsyncAuthorizationFilter
	{
		public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
		{
			string? token = context.HttpContext.Request.Headers.Authorization;

			if (string.IsNullOrEmpty(token) || await userService.GetByToken(token) == null)
				context.Result = new UnauthorizedResult();
		}
	}
}
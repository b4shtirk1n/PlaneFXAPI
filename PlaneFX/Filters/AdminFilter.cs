using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PlaneFX.Enums;
using PlaneFX.Interfaces;
using PlaneFX.Models;
using PlaneFX.Services;

namespace PlaneFX.Filters
{
    public class AdminFilter(UserService userService) : IAsyncAuthorizationFilter, IService
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            User? user = await userService.GetByToken(context.HttpContext.Request.Headers.Authorization!);

            if (user!.Role == (int)RoleEnum.Client)
                context.Result = new BadRequestResult();
        }
    }
}
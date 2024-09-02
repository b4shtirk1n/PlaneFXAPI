using PlaneFX.Models;

namespace PlaneFX.Middlewares
{
	public class DbConnectionMiddleware(PlaneFXContext dbContext) : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			if (await dbContext.Database.CanConnectAsync())
				await next.Invoke(context);
			else
				context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
		}
	}
}
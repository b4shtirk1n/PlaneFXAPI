using PlaneFX.DTOs;
using PlaneFX.Enums;
using PlaneFX.Interfaces;
using PlaneFX.Models;

namespace PlaneFX.Services
{
	public class StartupService(
		IConfiguration configuration,
		UserService userService,
		PlaneFXContext context
	) : IService
	{
		public const string TG_ID = "TG_ID";
		public const string TG_USERNAME = "TG_USERNAME";
		public const string TIME_ZONE = "TIME_ZONE";
		public const string TG_API_TOKEN = "TG_API_TOKEN";

		public async Task MakeSU()
		{
			long? id = configuration.GetValue<long>(TG_ID);
			string? username = configuration[TG_USERNAME];
			int? timeZone = configuration.GetValue<int>(TIME_ZONE);
			string? token = configuration[TG_API_TOKEN];

			if (id == null || string.IsNullOrEmpty(username) ||
				timeZone == null || string.IsNullOrEmpty(token))
				throw new NullReferenceException("do enter SA user in config!");

			try
			{
				await userService.GetAllByRole(RoleEnum.SU);
			}
			catch
			{
				await context.Database.EnsureCreatedAsync();
			}

			foreach (User user in await userService.GetAllByRole(RoleEnum.SU))
				await userService.ChangeRole(user, RoleEnum.Admin);

			if (await userService.GetByTg((long)id) is not User sa)
			{
				sa = await userService.Add(new UserDTO
				{
					TgId = (long)id,
					Username = username,
					TimeZone = (int)timeZone
				});
			}
			await userService.ChangeRole(sa, RoleEnum.SU);
		}
	}
}
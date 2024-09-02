using PlaneFX.DTOs;
using PlaneFX.Enums;
using PlaneFX.Models;

namespace PlaneFX.Services
{
	public class StartupService(IConfiguration configuration, UserService userService)
	{
		private const string TG_ID = "TG_ID";
		private const string TG_USERNAME = "TG_USERNAME";
		private const string TIME_ZONE = "TIME_ZONE";

		public async Task MakeSU()
		{
			string? id = configuration[TG_ID];
			string? username = configuration[TG_USERNAME];
			string? timeZone = configuration[TIME_ZONE];

			if (string.IsNullOrEmpty(id)
				|| string.IsNullOrEmpty(username)
				|| string.IsNullOrEmpty(timeZone))
				throw new NullReferenceException("do enter SA user in config!");

			foreach (User user in await userService.GetAllByRole(RoleEnum.SU))
				await userService.ChangeRole(user, RoleEnum.Admin);

			if (await userService.GetByTg(id) is not User sa)
			{
				sa = await userService.Add(new UserDTO
				{
					TgId = id,
					Username = username,
					TimeZone = timeZone
				});
			}
			await userService.ChangeRole(sa, RoleEnum.SU);
		}
	}
}
using PlaneFX.Enums;
using PlaneFX.Models;

namespace PlaneFX.Services
{
	public class AdminService(UserService userService)
	{
		public async Task MakeAdmin(User user)
			=> await userService.ChangeRole(user, RoleEnum.Admin);
	}
}
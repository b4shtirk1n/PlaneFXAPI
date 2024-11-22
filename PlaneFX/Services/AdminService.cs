using PlaneFX.Enums;
using PlaneFX.Interfaces;
using PlaneFX.Models;

namespace PlaneFX.Services
{
	public class AdminService(UserService userService) : IService
	{
		public async Task MakeAdmin(User user)
			=> await userService.ChangeRole(user, RoleEnum.Admin);
	}
}
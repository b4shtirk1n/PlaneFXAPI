using Microsoft.EntityFrameworkCore;
using PlaneFX.DTOs;
using PlaneFX.Enums;
using PlaneFX.Helpers;
using PlaneFX.Models;

namespace PlaneFX.Services
{
	public class UserService(PlaneFXContext context)
	{
		public async Task<User?> GetById(long id)
			=> await context.Users.FindAsync(id);

		public async Task<User?> GetByTg(long tgId)
			=> await context.Users.FirstOrDefaultAsync(u => u.TgId == tgId);

		public async Task<User?> GetByToken(string token)
			=> await context.Users.FirstOrDefaultAsync(u => u.Token == token);

		public async Task<IEnumerable<User>> GetAll()
			=> await context.Users.ToListAsync();

		public async Task<IEnumerable<User>> GetAllFromAdmin()
			=> await context.Users.Where(u => u.Role != (int)RoleEnum.SU).ToListAsync();

		public async Task<IEnumerable<User>> GetAllByRole(RoleEnum role)
			=> await context.Users.Where(u => u.Role == (int)role).ToListAsync();

		public async Task<bool> IsExist(long tgId)
			=> await context.Users.AnyAsync(u => u.TgId == tgId);

		public async Task<User> Add(UserDTO dTO)
		{
			var user = await context.Users.AddAsync(new User
			{
				Username = dTO.Username,
				TgId = dTO.TgId,
				Token = SecurityHelper.GenerateToken($"{dTO.TgId}"),
				Role = (int)RoleEnum.Client,
				Timezone = dTO.TimeZone
			});
			await context.SaveChangesAsync();
			return user.Entity;
		}

		public async Task<User> ChangeUsername(User user, string username)
		{
			user.Username = username;
			await context.SaveChangesAsync();
			return user;
		}

		public async Task ChangeRole(User user, RoleEnum role)
		{
			user.Role = (int)role;
			await context.SaveChangesAsync();
		}
	}
}
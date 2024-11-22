using Microsoft.EntityFrameworkCore;
using PlaneFX.DTOs;
using PlaneFX.Enums;
using PlaneFX.Helpers;
using PlaneFX.Interfaces;
using PlaneFX.Models;
using Telegram.Bot;

namespace PlaneFX.Services
{
	public class UserService(IConfiguration configuration, PlaneFXContext context) : IService
	{
		public async Task<string?> GetUserPhotoPath(long id)
		{
			var bot = new TelegramBotClient(configuration[StartupService.TG_API_TOKEN]!);
			var photos = await bot.GetUserProfilePhotos(id, limit: 1);

			if (photos.TotalCount == 0)
				return null;

			var photo = await bot.GetFile(photos.Photos[0][0].FileId);
			int lastSlash = photo.FilePath!.LastIndexOf('/');
			string name = photo.FilePath.Remove(0, lastSlash);
			string path = $"{Directory.GetCurrentDirectory()}/{photo.FilePath.Remove(lastSlash)}";
			string filePath = $"{path}{name}";

			Directory.CreateDirectory(path);
			await using var stream = File.Create(filePath);
			await bot.DownloadFile(photo.FilePath, stream);
			return filePath;
		}

		public async Task<User?> GetById(long id)
			=> await context.Users.FindAsync(id);

		public async Task<User?> GetByTg(long tgId)
			=> await context.Users.FirstOrDefaultAsync(u => u.TgId == tgId);

		public async Task<User?> GetByToken(string token)
			=> await context.Users.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Token == token);

		public async Task<IEnumerable<User>> Get()
			=> await context.Users.AsNoTracking()
				.ToListAsync();

		public async Task<IEnumerable<User>> GetAllFromAdmin()
			=> await context.Users.AsNoTracking()
				.Where(u => u.Role != (int)RoleEnum.SU)
				.ToListAsync();

		public async Task<IEnumerable<User>> GetAllByRole(RoleEnum role)
			=> await context.Users.AsNoTracking()
				.Where(u => u.Role == (int)role)
				.ToListAsync();

		public async Task<bool> IsExist(long tgId)
			=> await context.Users.AsNoTracking()
				.AnyAsync(u => u.TgId == tgId);

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
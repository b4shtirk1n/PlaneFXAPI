using System.ComponentModel.DataAnnotations;

namespace PlaneFX.DTOs
{
	public class UserDTO
	{
		[Required]
		[StringLength(100)]
		public required string Username { get; set; }

		[Required]
		public required long TgId { get; set; }

		[Required]
		public required int TimeZone { get; set; }
	}
}
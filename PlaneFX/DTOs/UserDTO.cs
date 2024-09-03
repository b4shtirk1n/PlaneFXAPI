using System.ComponentModel.DataAnnotations;

namespace PlaneFX.DTOs
{
	public class UserDTO
	{
		[Required]
		[StringLength(100)]
		public required string Username { get; set; }

		[Required]
		[StringLength(10)]
		public required string TgId { get; set; }

		[Required]
		[StringLength(3)]
		public required int TimeZone { get; set; }
	}
}
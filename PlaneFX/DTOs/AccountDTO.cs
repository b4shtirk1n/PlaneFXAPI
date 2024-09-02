using System.ComponentModel.DataAnnotations;

namespace PlaneFX.DTOs
{
	public class AccountDTO
	{
		[Required]
		[MaxLength(30)]
		public required string Name { get; set; }

		[Required]
		public required long User { get; set; }

		[Required]
		public required string Number { get; set; }

		[Required]
		public required bool IsCent { get; set; }
	}
}
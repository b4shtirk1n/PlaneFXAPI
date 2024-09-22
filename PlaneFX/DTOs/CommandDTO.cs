using System.ComponentModel.DataAnnotations;

namespace PlaneFX.DTOs
{
    public class CommandDTO
    {
        [Required]
        public required long Account { get; set; }

        public long? Order { get; set; }

        [StringLength(6)]
        public string? Ticker { get; set; }

        [Required]
        public required int Type { get; set; }
    }
}
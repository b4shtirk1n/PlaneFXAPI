using System.ComponentModel.DataAnnotations;

namespace PlaneFX.DTOs
{
    public class CommandDTO
    {
        [Required]
        public required long Account { get; set; }

        public long? Order { get; set; }

        [Required]
        public required int Ticker { get; set; }

        [Required]
        public required int Type { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace PlaneFX.Requests
{
    public class CommandRequest
    {
        [Required]
        public required long Timestamp { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required]
        public required long AccountNumber { get; set; }
    }
}
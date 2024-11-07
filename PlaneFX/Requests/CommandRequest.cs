using System.ComponentModel.DataAnnotations;

namespace PlaneFX.Requests
{
    public class CommandRequest
    {
        [Required]
        public required string Token { get; set; }

        [Required]
        public required long Id { get; set; }
    }
}
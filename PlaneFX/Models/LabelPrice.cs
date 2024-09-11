using System.ComponentModel.DataAnnotations;

namespace PlaneFX.Models
{
    public class LabelPrice
    {
        [Required]
        public required string Label { get; set; }

        [Required]
        public required int Amount { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.DTOs
{
    public class CommandDTO
    {
        [Required]
        public required long Account { get; set; }

        public long Order { get; set; }

        [Precision(10, 5)]
        public decimal Volume { get; set; }

        [StringLength(6)]
        public string? Ticker { get; set; }

        [Precision(10, 5)]
        public decimal? Price { get; set; }

        [Required]
        public required int Type { get; set; }

        [StringLength(15)]
        public required string OrderType { get; set; }
    }
}
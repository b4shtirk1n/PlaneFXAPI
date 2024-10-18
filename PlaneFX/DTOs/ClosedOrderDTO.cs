using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PlaneFX.DTOs
{
    public class ClosedOrderDTO
    {
        [Required]
        public required long Order { get; set; }

        [StringLength(15)]
        public string? Symbol { get; set; }

        [Required]
        [Precision(10, 5)]
        public required decimal Volume { get; set; }

        [Required]
        public required long TimeOpened { get; set; }

        [Required]
        public required long TimeClosed { get; set; }

        [Required]
        [Precision(10, 5)]
        public required decimal PriceOpened { get; set; }

        [Precision(10, 5)]
        public decimal Sl { get; set; }

        [Precision(10, 5)]
        public decimal Tp { get; set; }

        [Required]
        [Precision(10, 5)]
        public required decimal Swap { get; set; }

        [Required]
        [Precision(10, 5)]
        public required decimal Commissions { get; set; }

        [Required]
        [Precision(10, 5)]
        public required decimal Profit { get; set; }
    }
}
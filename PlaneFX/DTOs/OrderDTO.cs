using System.ComponentModel.DataAnnotations;

namespace PlaneFX.DTOs
{
    public class OrderDTO
    {
        [Required]
        public required long Timestamp { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required]
        public required string AccountName { get; set; }

        [Required]
        public required bool IsCent { get; set; }

        [Required]
        public required long AccountNumber { get; set; }

        [Required]
        public required string AccountOwner { get; set; }

        [Required]
        public required decimal Profit { get; set; }

        [Required]
        public required decimal ProfitToday { get; set; }

        [Required]
        public required decimal ProfitYesterday { get; set; }

        [Required]
        public required decimal ProfitWeek { get; set; }

        [Required]
        public required decimal MarginLevel { get; set; }

        [Required]
        public required decimal Drawdown { get; set; }

        [Required]
        public required decimal Balance { get; set; }

        [Required]
        public required List<OpenedOrderDTO> OpenedOrders { get; set; }

        public List<ClosedOrderDTO> ClosedOrders { get; set; } = [];
    }
}
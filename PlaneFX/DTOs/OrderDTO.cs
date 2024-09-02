using System.ComponentModel.DataAnnotations;

namespace PlaneFX.DTOs
{
    public class OrderDTO
    {
        [Required]
        public required long AccountNumber { get; set; }

        [Required]
        public required string AccountOwner { get; set; }

        [Required]
        public required List<OpenedOrderDTO> OpenedOrders { get; set; }

        public List<ClosedOrderDTO> ClosedOrders { get; set; } = [];
    }
}
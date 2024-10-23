using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PlaneFX.Models;

namespace PlaneFX.Requests
{
    public class InvoiceRequest
    {
        [Required]
        [StringLength(32)]
        public required string Name { get; set; }

        [Required]
        [StringLength(255)]
        public required string Description { get; set; }

        [Required]
        [StringLength(128)]
        public required string Payload { get; set; }

        [JsonPropertyName("provider_token")]
        public string? ProviderToken { get; set; }

        [Required]
        [StringLength(3)]
        public required string Currency { get; set; }

        [Required]
        public required LabelPrice[] Prices { get; set; }

        [JsonPropertyName("max_tip_amount")]
        public int? MTA { get; set; }

        [JsonPropertyName("suggested_tip_amounts")]
        public int[]? STA { get; set; }

        [JsonPropertyName("provider_data")]
        public string? ProviderData { get; set; }
    }
}
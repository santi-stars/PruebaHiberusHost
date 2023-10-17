using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PruebaHiberusHost.DTOs
{
    public class TransactionDTO
    {
        [Required]
        [JsonPropertyName("sku")]
        public string SKU { get; set; }

        [Required]
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [Required]
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        public override string ToString()
        {
            return $"SKU: {SKU} " + $"Amount: {Amount} " + $"Edad: {Currency}";
        }
    }
}

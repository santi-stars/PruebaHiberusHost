using PruebaHiberusHost.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PruebaHiberusHost.DTOs
{
    public class TransactionDTO
    {
        [JsonPropertyName("sku")]
        [Required]
        [SKUValidation]
        public string SKU { get; set; }
        [JsonPropertyName("amount")]
        [Required]
        public decimal Amount { get; set; }
        [JsonPropertyName("currency")]
        [Required]
        [CurrencyValidation]
        public string Currency { get; set; }
        public override string ToString()
        {
            return $"SKU: {SKU} " + $"Amount: {Amount} " + $"Edad: {Currency}";
        }
    }
}

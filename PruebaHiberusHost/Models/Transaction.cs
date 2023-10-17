using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PruebaHiberusHost.Entities
{
    public class Transaction
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

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
            return $"Id: {Id} " + $"SKU: {SKU} " + $"Amount: {Amount} " + $"Edad: {Currency}";
        }
    }
}

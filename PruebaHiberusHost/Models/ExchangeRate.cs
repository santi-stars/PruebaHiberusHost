using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PruebaHiberusHost.Entities
{
    public class ExchangeRate
    {
        [Key, Column(Order = 0)]
        [JsonPropertyName("fromCurrency")]
        public string FromCurrency { get; set; }

        [Key, Column(Order = 1)]
        [JsonPropertyName("toCurrency")]
        public string ToCurrency { get; set; }

        [Required]
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}

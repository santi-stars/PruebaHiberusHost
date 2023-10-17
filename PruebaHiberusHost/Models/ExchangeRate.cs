using PruebaHiberusHost.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PruebaHiberusHost.Entities
{
    public class ExchangeRate
    {
        [JsonPropertyName("fromCurrency")]
        [Key, Column(Order = 0)]
        [CurrencyValidation]    // Validación personalizada con formato 'XXX'
        public string FromCurrency { get; set; } // Moneda de origen (por ejemplo, "EUR")
        [JsonPropertyName("toCurrency")]
        [Key, Column(Order = 1)]
        [CurrencyValidation]    // Validación personalizada con formato 'XXX'
        public string ToCurrency { get; set; }   // Moneda de destino (por ejemplo, "USD")
        [JsonPropertyName("rate")]
        [Required]
        public decimal Rate { get; set; }        // Tasa de conversión (por ejemplo, 1.359)
    }
}

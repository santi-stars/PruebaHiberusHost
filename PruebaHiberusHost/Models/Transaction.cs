using PruebaHiberusHost.Validations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PruebaHiberusHost.Entities
{
    public class Transaction
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("sku")]
        [Required]
        [SKUValidation] // Validación personalizada con formato 'X0000'
        public string SKU { get; set; }     // Clave foránea que hace referencia a SKU en clase Sum
        [JsonPropertyName("amount")]
        [Required]
        public decimal Amount { get; set; }
        [JsonPropertyName("currency")]
        [Required]
        [CurrencyValidation]    // Validación personalizada con formato 'XXX'
        public string Currency { get; set; }
        [JsonPropertyName("sum")]
        [JsonIgnore]
        public Sum Sum { get; set; }        // Propiedad de navegación para la relación con Sum
        public override string ToString()
        {
            return $"Id: {Id} " + $"SKU: {SKU} " + $"Amount: {Amount} " + $"Edad: {Currency}";
        }
    }
}

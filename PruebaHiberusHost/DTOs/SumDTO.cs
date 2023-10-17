using PruebaHiberusHost.Validations;
using System.ComponentModel.DataAnnotations;

namespace PruebaHiberusHost.DTOs
{
    public class SumDTO
    {
        [Required]
        [SKUValidation]
        public string SKU { get; set; }
        [Required]  // Falta Validación Bank
        public decimal TotalSum { get; set; }
    }
}

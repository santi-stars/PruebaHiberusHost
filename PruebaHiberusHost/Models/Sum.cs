using PruebaHiberusHost.Validations;
using System.ComponentModel.DataAnnotations;

namespace PruebaHiberusHost.Entities
{
    public class Sum
    {
        [Required]
        [SKUValidation] // Validación personalizada con formato 'X0000'
        public string SKU { get; set; }     // Clave primaria que hace referencia a SKU en Transaction
        [Required]  // Falta Validación Bank
        public decimal TotalSum { get; set; }

        // Propiedad de navegación para la relación con Transaction 1:N
        public List<Transaction> Transactions { get; set; }
        public Sum() { TotalSum = 0; }
    }
}

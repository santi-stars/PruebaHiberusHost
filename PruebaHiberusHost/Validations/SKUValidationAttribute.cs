using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PruebaHiberusHost.Validations
{
    public class SKUValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {   // Comprueba que el valor es "null" o string vacío y devuelve Success
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;
            // Se crea el patrón para SKU con formato: 'X0000'
            string patron = "^[A-Z][0-9]{4}$";
            // Si no coincide el patrón devuelve un mensaje
            if (!Regex.IsMatch(value.ToString(), patron))
                return new ValidationResult("ERROR! 'SKU' tiene que tener el siguiente formato: 'X0000'");

            return ValidationResult.Success;
        }
    }
}

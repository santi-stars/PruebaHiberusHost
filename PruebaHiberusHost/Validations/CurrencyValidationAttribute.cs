using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PruebaHiberusHost.Validations
{
    public class CurrencyValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {   // Comprueba que el valor es "null" o string vacío y devuelve Success
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;
            // Se crea el patrón para 'Currency' con formato: 'XXX'
            string patron = "^[A-Z]{3}$";
            // Si no coincide el patrón devuelve un mensaje
            if (!Regex.IsMatch(value.ToString(), patron))
                return new ValidationResult("ERROR! La moneda tiene que tener el siguiente formato: 'XXX'");

            return ValidationResult.Success;
        }
    }
}

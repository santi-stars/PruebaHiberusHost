using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PruebaHiberusHost.DTOs;

namespace PruebaHiberus.ArchivosTXT
{
    public class Prueba
    {
        /*
        [HttpPost]
        public async Task<ActionResult> Post(TransactionDTO transactionDTO)
        {
            try
            {
                _logger.LogInformation("Creando Transacción");
                var transaction = mapper.Map<Transaction>(transactionDTO);
                decimal newRate = 1;
                string newCurrency = "JPY"; // Cambiar a otros para probar

                if (!transaction.Currency.Equals(newCurrency)) // Si la moneda actual es diferente de la nueva
                    newRate = calculateExchangeRate(transaction, newCurrency); // Calcula el nuevo ratio

                // Consulta si existe el "SKU" en la tabla Sum
                var oldSum = await _context.Sums.FirstOrDefaultAsync(sum => sum.SKU == transaction.SKU);

                if (oldSum != null)
                {
                    decimal newRoundedTotalSum = Math.Round((transaction.Amount * newRate), 2, MidpointRounding.ToEven); // Redondeo bancario a 2 decimales
                    oldSum.TotalSum += newRoundedTotalSum;
                    Console.WriteLine("oldSum: " + oldSum.TotalSum, "newRoundedTotalSum: " + newRoundedTotalSum); // BORRAR ######
                    _context.Update(oldSum);
                }
                else
                {
                    _context.Add(new Entities.Sum() { SKU = transaction.SKU, TotalSum = (transaction.Amount * newRate) });
                }

                _context.Add(transaction);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Registra el error en el registro (log) para facilitar la depuración.
                _logger.LogError(ex, "Error al procesar la solicitud.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }
        */
    }
}

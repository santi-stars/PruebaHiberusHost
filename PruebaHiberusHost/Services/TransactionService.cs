using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaHiberusHost.DTOs;
using PruebaHiberusHost.Entities;
using System.Text.Json;

namespace PruebaHiberusHost.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IExchangeRateUtility _exchangeRateUtility;
        private readonly ILogger<TransactionService> _logger;
        public TransactionService(ApplicationDbContext context,
                                  IExchangeRateUtility exchangeRateUtility,
                                  ILogger<TransactionService> logger)
        {
            _context = context;
            _exchangeRateUtility = exchangeRateUtility;
            _logger = logger;
        }


        /// <summary>
        /// Obtiene una lista de transacciones desde una URL especificada.
        /// </summary>
        /// <param name="url">La URL desde la cual se obtendrán los datos en formato JSON.</param>
        /// <returns>Una lista de objetos TransactionDTO que representan las transacciones.</returns>
        /// <exception cref="ArgumentException">Se lanza cuando la URL es nula o vacía.</exception>
        /// <exception cref="HttpRequestException">Se lanza en caso de errores de conexión o respuesta no exitosa.</exception>
        public async Task<List<TransactionDTO>> GetTransactionsFromUrlAsync(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url)) // Validación de la URL
                    throw new ArgumentException("La URL no puede estar vacía o nula.", nameof(url));

                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        var transactions = JsonSerializer.Deserialize<List<TransactionDTO>>(jsonContent);

                        foreach (var transaction in transactions)
                            _logger.LogInformation(transaction.ToString());

                        return transactions;
                    }
                    else
                    {
                        var statusCode = response.StatusCode;
                        var errorMessage = await response.Content.ReadAsStringAsync();

                        throw new HttpRequestException($"Error al obtener el JSON. Código de estado: " +
                                                       $"{statusCode}. Mensaje de error: {errorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener transacciones desde la URL.");
                throw;
            }
        }

        public async Task<ActionResult> CreateTransaction(TransactionDTO transactionDTO, ITransactionService _transactionService, IMapper mapper)
        {
            try
            {
                _logger.LogInformation("Creando Transacción");
                var transaction = mapper.Map<Transaction>(transactionDTO);

                decimal exchageRate = decimal.One;
                string startCurrency = transaction.Currency;
                string endCurrency = "JPY";     // Cambiar a otros para probar

                if (!startCurrency.Equals(endCurrency))  // Si la moneda de la transacción es diferente de la nueva
                    exchageRate = _exchangeRateUtility.CalculateExchangeRate(startCurrency, endCurrency);  // Calcula el nuevo ratio
                _logger.LogInformation("exchageRate " + exchageRate);

                var oldSum = await _context.Sums.FirstOrDefaultAsync(sum => sum.SKU == transaction.SKU);
                if (oldSum != null)
                {   // Si EXISTE SUMA Añade a la Suma total, la nueva cantidad convertida a la nueva moneda
                    decimal newRoundedTotalSum = Math.Round((transaction.Amount * exchageRate), 2, MidpointRounding.ToEven); // Redondeo BANK
                    oldSum.TotalSum += newRoundedTotalSum;
                    _logger.LogInformation("UPDATE oldSum " + oldSum + " newRoundedTotalSum " + newRoundedTotalSum);
                    _context.Update(oldSum);
                }   // NO EXISTE SUMA, hay que crear y añadir una suma con el mismo SKU (Fk en Transaction)
                else _context.Add(new Entities.Sum() { SKU = transaction.SKU, TotalSum = (transaction.Amount * exchageRate) });

                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la solicitud POST Transacción.");
                return null;
            }
        }
    }
}

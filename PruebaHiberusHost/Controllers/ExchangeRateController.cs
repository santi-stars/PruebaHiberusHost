using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaHiberusHost.Entities;
using PruebaHiberusHost.Responses;
using System.Text.Json;

namespace PruebaHiberusHost.Controllers
{
    /// <summary>
    /// Controlador para gestionar los ratios de cambio.
    /// </summary>
    [ApiController]
    [Route("api/rates")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransactionController> _logger;

        public ExchangeRateController(ApplicationDbContext context,
                                      ILogger<TransactionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los ratios de cambio de la base de datos, elimina un porcentaje aleatorio de filas y los mezcla de forma aleatoria antes de devolverlos.
        /// </summary>
        /// <returns>Una lista de ratios de cambio modificados de forma aleatoria.</returns>
        [HttpGet]
        public async Task<ActionResult<List<ExchangeRatesResponse>>> GetAllExchangeRates()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los ratios de cambio");
                var rates = await _context.ExchangeRates.ToListAsync();
                int totalRates = rates.Count;
                // Determina el porcentaje aleatorio entre 0% y 50% de filas a eliminar
                var percentageToRemove = new Random().NextDouble() * 0.5;
                var numberOfItemsToRemove = (int)(rates.Count * percentageToRemove);

                // Ordena la lista de forma aleatoria
                rates = rates.OrderBy(r => Guid.NewGuid()).ToList();

                // Toma las filas restantes después de eliminar el porcentaje deseado
                rates = rates.Take(rates.Count - numberOfItemsToRemove).ToList();

                // Construye el objeto de respuesta que incluye los ratios de cambio y el número de filas
                var response = new ExchangeRatesResponse
                {
                    ExchangeRates = rates,
                    ReturnedRows = $"Devuelve aleatoriamente {rates.Count} de {totalRates} filas en total"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los ratios de cambio.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        /*      MÉTODO DIRECTO
        
        /// <summary>
        /// Obtiene la lista de todos los ratios de cambio almacenados en la base de datos.
        /// </summary>
        /// <returns>ActionResult que contiene una lista de objetos ExchangeRate con la información de los ratios de cambio.</returns>
        [HttpGet]
        public async Task<ActionResult<List<ExchangeRate>>> GetAllExchangeRates()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los ratios de cambio");
                var rates = await _context.ExchangeRates.ToListAsync();
                return Ok(rates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los ratios de cambio.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        */
    }
}

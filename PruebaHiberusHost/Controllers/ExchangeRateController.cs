using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaHiberusHost.Entities;
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

        /// <summary>
        /// Crea un nuevo ratio de cambio en la base de datos.
        /// </summary>
        /// <param name="exchangeRate">El objeto ExchangeRate que contiene la información del nuevo ratio de cambio a crear.</param>
        /// <returns>ActionResult con el resultado de la operación. Devuelve BadRequest si el ratio de cambio ya existe, Ok si se crea con éxito.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateExchangeRate(ExchangeRate exchangeRate)
        {
            try
            {
                _logger.LogInformation($"Creando ratio de cambio {exchangeRate.FromCurrency} a {exchangeRate.ToCurrency}");
                var isExist = await _context.ExchangeRates.AnyAsync(x => x.FromCurrency == exchangeRate.FromCurrency &&
                                                                    x.ToCurrency == exchangeRate.ToCurrency);
                if (isExist)
                    return BadRequest($"El ratio de cambio {exchangeRate.FromCurrency} a {exchangeRate.ToCurrency} ya existe.");

                _context.Add(exchangeRate);
                await _context.SaveChangesAsync();
                return Ok(exchangeRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el ratio de cambio.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        /// <summary>
        /// Actualiza un ratio de cambio en la base de datos.
        /// </summary>
        /// <param name="exchangeRate">El objeto ExchangeRate que contiene la información del ratio de cambio a actualizar.</param>
        /// <returns>ActionResult con el resultado de la operación. Devuelve NotFound si el ratio de cambio no existe, Ok si se actualiza con éxito.</returns>
        [HttpPut]
        public async Task<ActionResult> UpdateExchangeRate([FromBody] ExchangeRate exchangeRate)
        {
            try
            {
                _logger.LogInformation($"Actualizando ratio de cambio {exchangeRate.FromCurrency} a {exchangeRate.ToCurrency}");
                var isExist = await _context.ExchangeRates.AnyAsync(x => x.FromCurrency == exchangeRate.FromCurrency &&
                                                                    x.ToCurrency == exchangeRate.ToCurrency);
                if (!isExist)
                    return NotFound($"El ratio de cambio {exchangeRate.FromCurrency} a {exchangeRate.ToCurrency} a actualizar no existe.");

                _context.Update(exchangeRate);
                await _context.SaveChangesAsync();
                return Ok(exchangeRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el ratio de cambio.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        /// <summary>
        /// Elimina un ratio de cambio de la base de datos según las monedas especificadas.
        /// </summary>
        /// <param name="FromCurrency">La moneda de origen del ratio de cambio a eliminar.</param>
        /// <param name="ToCurrency">La moneda de destino del ratio de cambio a eliminar.</param>
        /// <returns>ActionResult con el resultado de la operación. Devuelve NotFound si el ratio de cambio no existe, Ok si se elimina con éxito.</returns>
        [HttpDelete("{FromCurrency}/{ToCurrency}")]
        public async Task<ActionResult> DeleteExchangeRate([FromRoute] string FromCurrency, string ToCurrency)
        {
            try
            {
                _logger.LogInformation($"Eliminando ratio de cambio {FromCurrency} a {ToCurrency}");
                var isExist = await _context.ExchangeRates.AnyAsync(x => x.FromCurrency == FromCurrency &&
                                                                    x.ToCurrency == ToCurrency);
                if (!isExist)
                    return NotFound($"El ratio de cambio {FromCurrency} a {ToCurrency} a eliminar no existe.");

                var exchangeRate = await _context.ExchangeRates.FirstOrDefaultAsync(x => x.FromCurrency == FromCurrency &&
                                                                                    x.ToCurrency == ToCurrency);
                if (exchangeRate == null) return NotFound();

                _context.Remove(exchangeRate);
                await _context.SaveChangesAsync();

                return Ok(exchangeRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el ratio de cambio.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }
    }
}

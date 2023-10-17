using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaHiberusHost.DTOs;
using PruebaHiberusHost.Entities;
using PruebaHiberusHost.Services;
using PruebaHiberusHost.Validations;
using System.Text.Json;

namespace PruebaHiberusHost.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionController> _logger;
        private const string URL = "https://raw.githubusercontent.com/santi-stars/jsonhiberus/master/Transactions.json";

        public TransactionController(ITransactionService transactionService,
                                     ApplicationDbContext context,
                                     IMapper mapper,
                                     ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la lista de transacciones.
        /// </summary>
        /// <returns>Lista de transacciones en formato DTO.</returns>
        [HttpGet]
        public async Task<ActionResult<List<TransactionDTO>>> Get()
        {
            try
            {
                _logger.LogInformation("Obteniendo Transacciones");
                var transactions = await _context.Transactions.ToListAsync();
                return _mapper.Map<List<TransactionDTO>>(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las transacciones");
                return StatusCode(500, "Se produjo un error al procesar la solicitud.");
            }
        }

        /// <summary>
        /// Obtiene la lista de transacciones por SKU.
        /// </summary>
        /// <param name="SKU">SKU de la transacción.</param>
        /// <returns>Lista de transacciones en formato DTO.</returns>
        [HttpGet("{SKU}")]
        public async Task<ActionResult<List<TransactionDTO>>> GetBySKU([FromRoute][SKUValidation] string SKU)
        {
            try
            {
                _logger.LogInformation($"Obteniendo Transacciones por SKU = {SKU}");
                var IsExist = await _context.Transactions.AnyAsync(x => x.SKU == SKU);

                if (!IsExist) return StatusCode(404, $"No existe ninguna transaccion con SKU = {SKU}");

                var transactions = await _context.Transactions.Where(x => x.SKU.Contains(SKU)).ToListAsync();
                return _mapper.Map<List<TransactionDTO>>(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener las transacciones por SKU: {ex.Message}");
                return StatusCode(500, "Se produjo un error al procesar la solicitud.");
            }
        }

        /// <summary>
        /// Obtiene datos de transacciones desde una URL externa y los devuelve como DTO.
        /// </summary>
        /// <returns>Una lista de objetos TransactionDTO que representan las transacciones.</returns>
        [HttpGet]
        [Route("githubURL")]
        public async Task<ActionResult<List<TransactionDTO>>> GetJsonFromUrl()
        {
            try
            {
                _logger.LogInformation("Obteniendo Json de Transacciones a través de URL");
                var transactions = await _transactionService.GetTransactionsFromUrlAsync(URL);

                return _mapper.Map<List<TransactionDTO>>(transactions);
            }
            catch (Exception ex)
            {
                // INSERTAR MOCK AQUI----------------------------------------------------------------------------
                Console.WriteLine($"Error en la solicitud: {ex.Message}");
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(TransactionDTO transactionDTO)
        {
            try
            {
                _logger.LogInformation("Creando Transacción");
                await _transactionService.CreateTransaction(transactionDTO, _transactionService, _mapper);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la solicitud POST Transacción.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }
    }
}

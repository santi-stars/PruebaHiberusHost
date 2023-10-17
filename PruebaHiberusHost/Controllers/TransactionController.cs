using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaHiberusHost.DTOs;
using PruebaHiberusHost.Responses;

namespace PruebaHiberusHost.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ApplicationDbContext context,
                                     IMapper mapper,
                                     ILogger<TransactionController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una lista de transacciones con un porcentaje aleatorio de filas eliminadas y mezcladas.
        /// </summary>
        /// <returns>ActionResult que contiene una lista de objetos TransactionDTO con las transacciones.</returns>
        [HttpGet]
        public async Task<ActionResult<List<TransactionsResponse>>> Get()
        {
            try
            {
                _logger.LogInformation("Obteniendo Transacciones");
                var transactions = await _context.Transactions.ToListAsync();
                int totalTransactions = transactions.Count;
                // Determinar aleatoriamente el porcentaje de filas a eliminar
                Random rnd = new Random();
                int percentageToRemove = rnd.Next(10, 51); // Genera un número aleatorio entre 10 y 50 inclusive
                int rowsToRemove = transactions.Count * percentageToRemove / 100;

                // Eliminar las filas de forma aleatoria
                for (int i = 0; i < rowsToRemove; i++)
                {
                    int randomIndex = rnd.Next(transactions.Count);
                    transactions.RemoveAt(randomIndex);
                }

                // Mezclar las transacciones restantes de forma aleatoria
                transactions = transactions.OrderBy(t => rnd.Next()).ToList();

                var transactionsDTO = _mapper.Map<List<TransactionDTO>>(transactions);
                // Construye el objeto de respuesta que incluye los ratios de cambio y el número de filas
                var response = new TransactionsResponse
                {
                    TransactionDTOs = transactionsDTO,
                    ReturnedRows = $"Devuelve aleatoriamente {transactionsDTO.Count} de {totalTransactions} filas en total"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las transacciones");
                return StatusCode(500, "Se produjo un error al procesar la solicitud.");
            }
        }

        /*      MÉTODO DIRECTO
        
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

        */
    }
}

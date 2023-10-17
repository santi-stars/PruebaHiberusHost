using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaHiberusHost.DTOs;

namespace PruebaHiberusHost.Controllers
{

    [ApiController]
    [Route("api/sums")]
    public class SumController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionController> _logger;
        public SumController(ApplicationDbContext context,
                             IMapper mapper,
                             ILogger<TransactionController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una lista de todas las sumas disponibles.
        /// </summary>
        /// <returns>ActionResult que contiene una lista de objetos SumDTO que representan las sumas totales disponibles.</returns>
        [HttpGet]
        public async Task<ActionResult<List<SumDTO>>> GetAllSums()
        {
            try
            {
                _logger.LogInformation("Obteniendo Sumas");
                var sums = await _context.Sums.ToListAsync();

                return _mapper.Map<List<SumDTO>>(sums);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las sumas.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        /// <summary>
        /// Obtiene la suma total por SKU.
        /// </summary>
        /// <param name="SKU">El SKU del producto para el cual se desea obtener la suma total.</param>
        /// <returns>ActionResult que contiene un objeto SumDTO con la suma total por SKU.</returns>
        [HttpGet("{SKU}", Name = "obtenerSuma")]
        public async Task<ActionResult<SumDTO>> GetSumBySKU(string SKU)
        {
            try
            {
                _logger.LogInformation($"Obteniendo Suma total por SKU = {SKU}");
                var sum = await _context.Sums.FirstOrDefaultAsync(s => s.SKU == SKU);

                if (sum == null) return NotFound();

                return _mapper.Map<SumDTO>(sum);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la suma total por SKU.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }
    }
}

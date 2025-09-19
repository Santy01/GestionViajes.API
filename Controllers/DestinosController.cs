using Microsoft.AspNetCore.Mvc;
using GestionViajes.API.DTOs;
using GestionViajes.API.Interfaces;

namespace GestionViajes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DestinosController : ControllerBase
    {
        private readonly IDestinoService _destinoService;
        private readonly ILogger<DestinosController> _logger;

        public DestinosController(IDestinoService destinoService, ILogger<DestinosController> logger)
        {
            _destinoService = destinoService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los destinos turísticos
        /// </summary>
        /// <returns>Lista de destinos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DestinoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DestinoDto>>> GetDestinos()
        {
            try
            {
                var destinos = await _destinoService.GetAllDestinosAsync();
                return Ok(destinos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener destinos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un destino específico por su ID
        /// </summary>
        /// <param name="id">ID del destino</param>
        /// <returns>Destino encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DestinoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DestinoDto>> GetDestino(int id)
        {
            try
            {
                var destino = await _destinoService.GetDestinoByIdAsync(id);
                
                if (destino == null)
                {
                    return NotFound($"No se encontró el destino con ID {id}");
                }

                return Ok(destino);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener destino con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene destinos por país
        /// </summary>
        /// <param name="pais">Nombre del país</param>
        /// <returns>Lista de destinos del país especificado</returns>
        [HttpGet("pais/{pais}")]
        [ProducesResponseType(typeof(IEnumerable<DestinoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DestinoDto>>> GetDestinosByPais(string pais)
        {
            try
            {
                var destinos = await _destinoService.GetDestinosByPaisAsync(pais);
                return Ok(destinos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener destinos por país {Pais}", pais);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene destinos dentro de un rango de costo
        /// </summary>
        /// <param name="minCost">Costo mínimo</param>
        /// <param name="maxCost">Costo máximo</param>
        /// <returns>Lista de destinos dentro del rango de precio</returns>
        [HttpGet("costo")]
        [ProducesResponseType(typeof(IEnumerable<DestinoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DestinoDto>>> GetDestinosByCostRange(
            [FromQuery] decimal minCost = 0, 
            [FromQuery] decimal maxCost = decimal.MaxValue)
        {
            try
            {
                if (minCost < 0 || maxCost < 0 || minCost > maxCost)
                {
                    return BadRequest("El rango de costos no es válido");
                }

                var destinos = await _destinoService.GetDestinosByCostRangeAsync(minCost, maxCost);
                return Ok(destinos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener destinos por rango de costo");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Busca destinos por término de búsqueda
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de destinos que coinciden con el término</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<DestinoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DestinoDto>>> SearchDestinos([FromQuery] string searchTerm = "")
        {
            try
            {
                var destinos = await _destinoService.SearchDestinosAsync(searchTerm);
                return Ok(destinos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar destinos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea un nuevo destino turístico
        /// </summary>
        /// <param name="destinoCreateDto">Datos del destino a crear</param>
        /// <returns>Destino creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(DestinoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DestinoDto>> CreateDestino([FromBody] DestinoCreateDto destinoCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var destino = await _destinoService.CreateDestinoAsync(destinoCreateDto);
                return CreatedAtAction(nameof(GetDestino), new { id = destino.DestinoId }, destino);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear destino");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza un destino existente
        /// </summary>
        /// <param name="id">ID del destino a actualizar</param>
        /// <param name="destinoUpdateDto">Datos actualizados del destino</param>
        /// <returns>Destino actualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DestinoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DestinoDto>> UpdateDestino(int id, [FromBody] DestinoUpdateDto destinoUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var destino = await _destinoService.UpdateDestinoAsync(id, destinoUpdateDto);
                
                if (destino == null)
                {
                    return NotFound($"No se encontró el destino con ID {id}");
                }

                return Ok(destino);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar destino con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina un destino específico
        /// </summary>
        /// <param name="id">ID del destino a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDestino(int id)
        {
            try
            {
                var deleted = await _destinoService.DeleteDestinoAsync(id);
                
                if (!deleted)
                {
                    return NotFound($"No se encontró el destino con ID {id}");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar destino con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Verifica si existe un destino con el ID especificado
        /// </summary>
        /// <param name="id">ID del destino</param>
        /// <returns>True si existe, false en caso contrario</returns>
        [HttpHead("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DestinoExists(int id)
        {
            try
            {
                var exists = await _destinoService.ExistsAsync(id);
                return exists ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del destino con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
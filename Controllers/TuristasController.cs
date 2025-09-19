using Microsoft.AspNetCore.Mvc;
using GestionViajes.API.DTOs;
using GestionViajes.API.Interfaces;

namespace GestionViajes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TuristasController : ControllerBase
    {
        private readonly ITuristaService _turistaService;
        private readonly ILogger<TuristasController> _logger;

        public TuristasController(ITuristaService turistaService, ILogger<TuristasController> logger)
        {
            _turistaService = turistaService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los turistas registrados
        /// </summary>
        /// <returns>Lista de turistas</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TuristaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TuristaDto>>> GetTuristas()
        {
            try
            {
                var turistas = await _turistaService.GetAllTuristasAsync();
                return Ok(turistas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener turistas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un turista específico por su ID
        /// </summary>
        /// <param name="id">ID del turista</param>
        /// <returns>Turista encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TuristaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TuristaDto>> GetTurista(int id)
        {
            try
            {
                var turista = await _turistaService.GetTuristaByIdAsync(id);
                
                if (turista == null)
                {
                    return NotFound($"No se encontró el turista con ID {id}");
                }

                return Ok(turista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener turista con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un turista por su email
        /// </summary>
        /// <param name="email">Email del turista</param>
        /// <returns>Turista encontrado</returns>
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(TuristaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TuristaDto>> GetTuristaByEmail(string email)
        {
            try
            {
                var turista = await _turistaService.GetTuristaByEmailAsync(email);
                
                if (turista == null)
                {
                    return NotFound($"No se encontró el turista con email {email}");
                }

                return Ok(turista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener turista por email {Email}", email);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Busca turistas por término de búsqueda
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de turistas que coinciden con el término</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<TuristaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TuristaDto>>> SearchTuristas([FromQuery] string searchTerm = "")
        {
            try
            {
                var turistas = await _turistaService.SearchTuristasAsync(searchTerm);
                return Ok(turistas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar turistas");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea un nuevo turista
        /// </summary>
        /// <param name="turistaCreateDto">Datos del turista a crear</param>
        /// <returns>Turista creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TuristaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TuristaDto>> CreateTurista([FromBody] TuristaCreateDto turistaCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var turista = await _turistaService.CreateTuristaAsync(turistaCreateDto);
                return CreatedAtAction(nameof(GetTurista), new { id = turista.TuristaId }, turista);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear turista");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza un turista existente
        /// </summary>
        /// <param name="id">ID del turista a actualizar</param>
        /// <param name="turistaUpdateDto">Datos actualizados del turista</param>
        /// <returns>Turista actualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TuristaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TuristaDto>> UpdateTurista(int id, [FromBody] TuristaUpdateDto turistaUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var turista = await _turistaService.UpdateTuristaAsync(id, turistaUpdateDto);
                
                if (turista == null)
                {
                    return NotFound($"No se encontró el turista con ID {id}");
                }

                return Ok(turista);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar turista con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina un turista específico
        /// </summary>
        /// <param name="id">ID del turista a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTurista(int id)
        {
            try
            {
                var deleted = await _turistaService.DeleteTuristaAsync(id);
                
                if (!deleted)
                {
                    return NotFound($"No se encontró el turista con ID {id}");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar turista con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Verifica si existe un turista con el ID especificado
        /// </summary>
        /// <param name="id">ID del turista</param>
        /// <returns>True si existe, false en caso contrario</returns>
        [HttpHead("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TuristaExists(int id)
        {
            try
            {
                var exists = await _turistaService.ExistsAsync(id);
                return exists ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del turista con ID {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Verifica si existe un turista con el email especificado
        /// </summary>
        /// <param name="email">Email del turista</param>
        /// <returns>True si existe, false en caso contrario</returns>
        [HttpHead("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TuristaExistsByEmail(string email)
        {
            try
            {
                var exists = await _turistaService.ExistsByEmailAsync(email);
                return exists ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del turista con email {Email}", email);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
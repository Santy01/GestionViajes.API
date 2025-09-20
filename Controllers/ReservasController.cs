using Microsoft.AspNetCore.Mvc;
using GestionViajes.API.DTOs;
using GestionViajes.API.Interfaces;

namespace GestionViajes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly ILogger<ReservasController> _logger;

        public ReservasController(IReservaService reservaService, ILogger<ReservasController> logger)
        {
            _reservaService = reservaService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReservaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReservaDto>>> GetReservas()
        {
            var reservas = await _reservaService.GetAllReservasAsync();
            return Ok(reservas);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReservaDto>> GetReserva(int id)
        {
            var reserva = await _reservaService.GetReservaByIdAsync(id);
            if (reserva == null) return NotFound();
            return Ok(reserva);
        }

        [HttpGet("turista/{turistaId}")]
        [ProducesResponseType(typeof(IEnumerable<ReservaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReservaDto>>> GetByTurista(int turistaId)
        {
            var reservas = await _reservaService.GetReservasByTuristaAsync(turistaId);
            return Ok(reservas);
        }

        [HttpGet("destino/{destinoId}")]
        [ProducesResponseType(typeof(IEnumerable<ReservaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReservaDto>>> GetByDestino(int destinoId)
        {
            var reservas = await _reservaService.GetReservasByDestinoAsync(destinoId);
            return Ok(reservas);
        }

        [HttpGet("rango-fechas")]
        [ProducesResponseType(typeof(IEnumerable<ReservaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ReservaDto>>> GetByDateRange([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            try
            {
                var reservas = await _reservaService.GetReservasByDateRangeAsync(desde, hasta);
                return Ok(reservas);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ReservaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ReservaDto>> CreateReserva([FromBody] ReservaCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var created = await _reservaService.CreateReservaAsync(dto);
                return CreatedAtAction(nameof(GetReserva), new { id = created.ReservaId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ReservaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ReservaDto>> UpdateReserva(int id, [FromBody] ReservaUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var updated = await _reservaService.UpdateReservaAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var deleted = await _reservaService.DeleteReservaAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpHead("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReservaExists(int id)
        {
            var exists = await _reservaService.ExistsAsync(id);
            return exists ? Ok() : NotFound();
        }
    }
}
using GestionViajes.API.DTOs;
using GestionViajes.API.Interfaces;
using GestionViajes.API.Models;

namespace GestionViajes.API.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly ITuristaRepository _turistaRepository;
        private readonly IDestinoRepository _destinoRepository;
        private readonly ILogger<ReservaService> _logger;

        public ReservaService(
            IReservaRepository reservaRepository,
            ITuristaRepository turistaRepository,
            IDestinoRepository destinoRepository,
            ILogger<ReservaService> logger)
        {
            _reservaRepository = reservaRepository;
            _turistaRepository = turistaRepository;
            _destinoRepository = destinoRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ReservaDto>> GetAllReservasAsync()
        {
            var reservas = await _reservaRepository.GetAllAsync();
            return reservas.Select(MapToDto);
        }

        public async Task<ReservaDto?> GetReservaByIdAsync(int id)
        {
            var reserva = await _reservaRepository.GetByIdAsync(id);
            return reserva != null ? MapToDto(reserva) : null;
        }

        public async Task<IEnumerable<ReservaDto>> GetReservasByTuristaAsync(int turistaId)
        {
            var reservas = await _reservaRepository.GetByTuristaAsync(turistaId);
            return reservas.Select(MapToDto);
        }

        public async Task<IEnumerable<ReservaDto>> GetReservasByDestinoAsync(int destinoId)
        {
            var reservas = await _reservaRepository.GetByDestinoAsync(destinoId);
            return reservas.Select(MapToDto);
        }

        public async Task<IEnumerable<ReservaDto>> GetReservasByDateRangeAsync(DateTime desde, DateTime hasta)
        {
            if (desde >= hasta)
                throw new ArgumentException("El rango de fechas no es válido");

            var reservas = await _reservaRepository.GetByDateRangeAsync(desde, hasta);
            return reservas.Select(MapToDto);
        }

        public async Task<ReservaDto> CreateReservaAsync(ReservaCreateDto dto)
        {
            // Validaciones
            if (dto.FechaInicio >= dto.FechaFin)
                throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio");

            if (!await _turistaRepository.ExistsAsync(dto.TuristaId))
                throw new InvalidOperationException($"No existe Turista con ID {dto.TuristaId}");

            var destino = await _destinoRepository.GetByIdAsync(dto.DestinoId);
            if (destino == null)
                throw new InvalidOperationException($"No existe Destino con ID {dto.DestinoId}");

            // Verificar solape de reservas del turista
            var overlapping = await _reservaRepository.ExistsOverlappingAsync(dto.TuristaId, dto.FechaInicio, dto.FechaFin);
            if (overlapping)
                throw new InvalidOperationException("El turista ya tiene una reserva que se solapa en esas fechas");

            // Calcular total simple: costo destino * cantidad de personas * días
            var dias = (dto.FechaFin.Date - dto.FechaInicio.Date).Days;
            if (dias <= 0) dias = 1;
            var total = destino.Costo * dto.CantidadPersonas * dias;

            var reserva = new Reserva
            {
                TuristaId = dto.TuristaId,
                DestinoId = dto.DestinoId,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                CantidadPersonas = dto.CantidadPersonas,
                Total = total,
                FechaCreacion = DateTime.UtcNow
            };

            var created = await _reservaRepository.AddAsync(reserva);
            _logger.LogInformation("Reserva creada: ID {Id}", created.ReservaId);
            return MapToDto(created);
        }

        public async Task<ReservaDto?> UpdateReservaAsync(int id, ReservaUpdateDto dto)
        {
            if (dto.FechaInicio >= dto.FechaFin)
                throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio");

            var existing = await _reservaRepository.GetByIdAsync(id);
            if (existing == null) return null;

            // Verificar solape con otras reservas del mismo turista
            var overlapping = await _reservaRepository.ExistsOverlappingAsync(existing.TuristaId, dto.FechaInicio, dto.FechaFin, id);
            if (overlapping)
                throw new InvalidOperationException("El turista ya tiene otra reserva que se solapa en esas fechas");

            // Recalcular total según destino actual de la reserva
            var destino = await _destinoRepository.GetByIdAsync(existing.DestinoId);
            if (destino == null)
                throw new InvalidOperationException($"No existe Destino con ID {existing.DestinoId}");

            var dias = (dto.FechaFin.Date - dto.FechaInicio.Date).Days;
            if (dias <= 0) dias = 1;
            existing.Total = destino.Costo * dto.CantidadPersonas * dias;

            existing.FechaInicio = dto.FechaInicio;
            existing.FechaFin = dto.FechaFin;
            existing.CantidadPersonas = dto.CantidadPersonas;
            existing.FechaActualizacion = DateTime.UtcNow;

            var updated = await _reservaRepository.UpdateAsync(existing);
            _logger.LogInformation("Reserva actualizada: ID {Id}", id);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteReservaAsync(int id)
        {
            var deleted = await _reservaRepository.DeleteAsync(id);
            if (deleted)
                _logger.LogInformation("Reserva eliminada: ID {Id}", id);
            return deleted;
        }

        public async Task<bool> ExistsAsync(int id) => await _reservaRepository.ExistsAsync(id);

        private static ReservaDto MapToDto(Reserva r)
        {
            return new ReservaDto
            {
                ReservaId = r.ReservaId,
                TuristaId = r.TuristaId,
                DestinoId = r.DestinoId,
                FechaInicio = r.FechaInicio,
                FechaFin = r.FechaFin,
                CantidadPersonas = r.CantidadPersonas,
                Total = r.Total,
                FechaCreacion = r.FechaCreacion,
                FechaActualizacion = r.FechaActualizacion
            };
        }
    }
}
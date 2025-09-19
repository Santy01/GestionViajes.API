using GestionViajes.API.DTOs;
using GestionViajes.API.Interfaces;
using GestionViajes.API.Models;

namespace GestionViajes.API.Services
{
    public class TuristaService : ITuristaService
    {
        private readonly ITuristaRepository _turistaRepository;
        private readonly ILogger<TuristaService> _logger;

        public TuristaService(ITuristaRepository turistaRepository, ILogger<TuristaService> logger)
        {
            _turistaRepository = turistaRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TuristaDto>> GetAllTuristasAsync()
        {
            try
            {
                var turistas = await _turistaRepository.GetAllAsync();
                return turistas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los turistas");
                throw;
            }
        }

        public async Task<TuristaDto?> GetTuristaByIdAsync(int id)
        {
            try
            {
                var turista = await _turistaRepository.GetByIdAsync(id);
                return turista != null ? MapToDto(turista) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el turista con ID {Id}", id);
                throw;
            }
        }

        public async Task<TuristaDto?> GetTuristaByEmailAsync(string email)
        {
            try
            {
                var turista = await _turistaRepository.GetByEmailAsync(email);
                return turista != null ? MapToDto(turista) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener turista por email {Email}", email);
                throw;
            }
        }

        public async Task<IEnumerable<TuristaDto>> SearchTuristasAsync(string searchTerm)
        {
            try
            {
                var turistas = await _turistaRepository.SearchTuristasAsync(searchTerm);
                return turistas.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar turistas con término {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<TuristaDto> CreateTuristaAsync(TuristaCreateDto turistaCreateDto)
        {
            try
            {
                // Validar si ya existe un turista con el mismo email
                var exists = await _turistaRepository.ExistsByEmailAsync(turistaCreateDto.Email);
                if (exists)
                {
                    throw new InvalidOperationException($"Ya existe un turista registrado con el email '{turistaCreateDto.Email}'");
                }

                var turista = new Turista
                {
                    Nombre = turistaCreateDto.Nombre,
                    Apellido = turistaCreateDto.Apellido,
                    Email = turistaCreateDto.Email,
                    Telefono = turistaCreateDto.Telefono,
                    FechaRegistro = DateTime.UtcNow
                };

                var createdTurista = await _turistaRepository.AddAsync(turista);
                _logger.LogInformation("Turista creado exitosamente: {Nombre} {Apellido}, {Email}", 
                    createdTurista.Nombre, createdTurista.Apellido, createdTurista.Email);
                
                return MapToDto(createdTurista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear turista {Nombre} {Apellido}, {Email}", 
                    turistaCreateDto.Nombre, turistaCreateDto.Apellido, turistaCreateDto.Email);
                throw;
            }
        }

        public async Task<TuristaDto?> UpdateTuristaAsync(int id, TuristaUpdateDto turistaUpdateDto)
        {
            try
            {
                var existingTurista = await _turistaRepository.GetByIdAsync(id);
                if (existingTurista == null)
                {
                    return null;
                }

                // Validar si ya existe otro turista con el mismo email
                var emailExists = await _turistaRepository.ExistsByEmailAsync(turistaUpdateDto.Email, id);
                if (emailExists)
                {
                    throw new InvalidOperationException($"Ya existe otro turista registrado con el email '{turistaUpdateDto.Email}'");
                }

                existingTurista.Nombre = turistaUpdateDto.Nombre;
                existingTurista.Apellido = turistaUpdateDto.Apellido;
                existingTurista.Email = turistaUpdateDto.Email;
                existingTurista.Telefono = turistaUpdateDto.Telefono;
                existingTurista.FechaActualizacion = DateTime.UtcNow;

                var updatedTurista = await _turistaRepository.UpdateAsync(existingTurista);
                _logger.LogInformation("Turista actualizado exitosamente: ID {Id}", id);
                
                return MapToDto(updatedTurista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar turista con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteTuristaAsync(int id)
        {
            try
            {
                var deleted = await _turistaRepository.DeleteAsync(id);
                if (deleted)
                {
                    _logger.LogInformation("Turista eliminado exitosamente: ID {Id}", id);
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar turista con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _turistaRepository.ExistsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del turista con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            try
            {
                return await _turistaRepository.ExistsByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del turista con email {Email}", email);
                throw;
            }
        }

        private static TuristaDto MapToDto(Turista turista)
        {
            return new TuristaDto
            {
                TuristaId = turista.TuristaId,
                Nombre = turista.Nombre,
                Apellido = turista.Apellido,
                Email = turista.Email,
                Telefono = turista.Telefono,
                FechaRegistro = turista.FechaRegistro,
                FechaActualizacion = turista.FechaActualizacion,
                NombreCompleto = turista.NombreCompleto
            };
        }
    }
}
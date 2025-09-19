using GestionViajes.API.DTOs;
using GestionViajes.API.Interfaces;
using GestionViajes.API.Models;

namespace GestionViajes.API.Services
{
    public class DestinoService : IDestinoService
    {
        private readonly IDestinoRepository _destinoRepository;
        private readonly ILogger<DestinoService> _logger;

        public DestinoService(IDestinoRepository destinoRepository, ILogger<DestinoService> logger)
        {
            _destinoRepository = destinoRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<DestinoDto>> GetAllDestinosAsync()
        {
            try
            {
                var destinos = await _destinoRepository.GetAllAsync();
                return destinos.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los destinos");
                throw;
            }
        }

        public async Task<DestinoDto?> GetDestinoByIdAsync(int id)
        {
            try
            {
                var destino = await _destinoRepository.GetByIdAsync(id);
                return destino != null ? MapToDto(destino) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el destino con ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<DestinoDto>> GetDestinosByPaisAsync(string pais)
        {
            try
            {
                var destinos = await _destinoRepository.GetDestinosByPaisAsync(pais);
                return destinos.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener destinos por país {Pais}", pais);
                throw;
            }
        }

        public async Task<IEnumerable<DestinoDto>> GetDestinosByCostRangeAsync(decimal minCost, decimal maxCost)
        {
            try
            {
                if (minCost < 0 || maxCost < 0 || minCost > maxCost)
                {
                    throw new ArgumentException("El rango de costos no es válido");
                }

                var destinos = await _destinoRepository.GetDestinosByCostRangeAsync(minCost, maxCost);
                return destinos.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener destinos por rango de costo {MinCost}-{MaxCost}", minCost, maxCost);
                throw;
            }
        }

        public async Task<IEnumerable<DestinoDto>> SearchDestinosAsync(string searchTerm)
        {
            try
            {
                var destinos = await _destinoRepository.SearchDestinosAsync(searchTerm);
                return destinos.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar destinos con término {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<DestinoDto> CreateDestinoAsync(DestinoCreateDto destinoCreateDto)
        {
            try
            {
                // Validar si ya existe un destino con el mismo nombre y país
                var exists = await _destinoRepository.ExistsByNameAndCountryAsync(destinoCreateDto.Nombre, destinoCreateDto.Pais);
                if (exists)
                {
                    throw new InvalidOperationException($"Ya existe un destino llamado '{destinoCreateDto.Nombre}' en {destinoCreateDto.Pais}");
                }

                var destino = new Destino
                {
                    Nombre = destinoCreateDto.Nombre,
                    Pais = destinoCreateDto.Pais,
                    Descripcion = destinoCreateDto.Descripcion,
                    Costo = destinoCreateDto.Costo,
                    FechaCreacion = DateTime.UtcNow
                };

                var createdDestino = await _destinoRepository.AddAsync(destino);
                _logger.LogInformation("Destino creado exitosamente: {Nombre}, {Pais}", createdDestino.Nombre, createdDestino.Pais);
                
                return MapToDto(createdDestino);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear destino {Nombre}, {Pais}", destinoCreateDto.Nombre, destinoCreateDto.Pais);
                throw;
            }
        }

        public async Task<DestinoDto?> UpdateDestinoAsync(int id, DestinoUpdateDto destinoUpdateDto)
        {
            try
            {
                var existingDestino = await _destinoRepository.GetByIdAsync(id);
                if (existingDestino == null)
                {
                    return null;
                }

                // Validar si ya existe otro destino con el mismo nombre y país
                var duplicateExists = await _destinoRepository.FindAsync(d => 
                    d.DestinoId != id && 
                    d.Nombre.ToLower() == destinoUpdateDto.Nombre.ToLower() && 
                    d.Pais.ToLower() == destinoUpdateDto.Pais.ToLower());

                if (duplicateExists.Any())
                {
                    throw new InvalidOperationException($"Ya existe otro destino llamado '{destinoUpdateDto.Nombre}' en {destinoUpdateDto.Pais}");
                }

                existingDestino.Nombre = destinoUpdateDto.Nombre;
                existingDestino.Pais = destinoUpdateDto.Pais;
                existingDestino.Descripcion = destinoUpdateDto.Descripcion;
                existingDestino.Costo = destinoUpdateDto.Costo;
                existingDestino.FechaActualizacion = DateTime.UtcNow;

                var updatedDestino = await _destinoRepository.UpdateAsync(existingDestino);
                _logger.LogInformation("Destino actualizado exitosamente: ID {Id}", id);
                
                return MapToDto(updatedDestino);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar destino con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteDestinoAsync(int id)
        {
            try
            {
                var deleted = await _destinoRepository.DeleteAsync(id);
                if (deleted)
                {
                    _logger.LogInformation("Destino eliminado exitosamente: ID {Id}", id);
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar destino con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _destinoRepository.ExistsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del destino con ID {Id}", id);
                throw;
            }
        }

        private static DestinoDto MapToDto(Destino destino)
        {
            return new DestinoDto
            {
                DestinoId = destino.DestinoId,
                Nombre = destino.Nombre,
                Pais = destino.Pais,
                Descripcion = destino.Descripcion,
                Costo = destino.Costo,
                FechaCreacion = destino.FechaCreacion,
                FechaActualizacion = destino.FechaActualizacion
            };
        }
    }
}
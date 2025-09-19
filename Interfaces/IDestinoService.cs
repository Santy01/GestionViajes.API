using GestionViajes.API.DTOs;

namespace GestionViajes.API.Interfaces
{
    public interface IDestinoService
    {
        Task<IEnumerable<DestinoDto>> GetAllDestinosAsync();
        Task<DestinoDto?> GetDestinoByIdAsync(int id);
        Task<IEnumerable<DestinoDto>> GetDestinosByPaisAsync(string pais);
        Task<IEnumerable<DestinoDto>> GetDestinosByCostRangeAsync(decimal minCost, decimal maxCost);
        Task<IEnumerable<DestinoDto>> SearchDestinosAsync(string searchTerm);
        Task<DestinoDto> CreateDestinoAsync(DestinoCreateDto destinoCreateDto);
        Task<DestinoDto?> UpdateDestinoAsync(int id, DestinoUpdateDto destinoUpdateDto);
        Task<bool> DeleteDestinoAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
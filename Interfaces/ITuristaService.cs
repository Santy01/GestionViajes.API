using GestionViajes.API.DTOs;

namespace GestionViajes.API.Interfaces
{
    public interface ITuristaService
    {
        Task<IEnumerable<TuristaDto>> GetAllTuristasAsync();
        Task<TuristaDto?> GetTuristaByIdAsync(int id);
        Task<TuristaDto?> GetTuristaByEmailAsync(string email);
        Task<IEnumerable<TuristaDto>> SearchTuristasAsync(string searchTerm);
        Task<TuristaDto> CreateTuristaAsync(TuristaCreateDto turistaCreateDto);
        Task<TuristaDto?> UpdateTuristaAsync(int id, TuristaUpdateDto turistaUpdateDto);
        Task<bool> DeleteTuristaAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByEmailAsync(string email);
    }
}
using GestionViajes.API.DTOs;

namespace GestionViajes.API.Interfaces
{
    public interface IReservaService
    {
        Task<IEnumerable<ReservaDto>> GetAllReservasAsync();
        Task<ReservaDto?> GetReservaByIdAsync(int id);
        Task<IEnumerable<ReservaDto>> GetReservasByTuristaAsync(int turistaId);
        Task<IEnumerable<ReservaDto>> GetReservasByDestinoAsync(int destinoId);
        Task<IEnumerable<ReservaDto>> GetReservasByDateRangeAsync(DateTime desde, DateTime hasta);
        Task<ReservaDto> CreateReservaAsync(ReservaCreateDto reservaCreateDto);
        Task<ReservaDto?> UpdateReservaAsync(int id, ReservaUpdateDto reservaUpdateDto);
        Task<bool> DeleteReservaAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
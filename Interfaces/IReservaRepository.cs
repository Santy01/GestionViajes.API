using GestionViajes.API.Models;

namespace GestionViajes.API.Interfaces
{
    public interface IReservaRepository : IGenericRepository<Reserva>
    {
        Task<IEnumerable<Reserva>> GetByTuristaAsync(int turistaId);
        Task<IEnumerable<Reserva>> GetByDestinoAsync(int destinoId);
        Task<IEnumerable<Reserva>> GetByDateRangeAsync(DateTime desde, DateTime hasta);
        Task<bool> ExistsOverlappingAsync(int turistaId, DateTime inicio, DateTime fin, int? excludeReservaId = null);
    }
}
using GestionViajes.API.Models;

namespace GestionViajes.API.Interfaces
{
    public interface ITuristaRepository : IGenericRepository<Turista>
    {
        Task<Turista?> GetByEmailAsync(string email);
        Task<IEnumerable<Turista>> SearchTuristasAsync(string searchTerm);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email, int excludeId);
    }
}
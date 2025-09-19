using GestionViajes.API.Models;

namespace GestionViajes.API.Interfaces
{
    public interface IDestinoRepository : IGenericRepository<Destino>
    {
        Task<IEnumerable<Destino>> GetDestinosByPaisAsync(string pais);
        Task<IEnumerable<Destino>> GetDestinosByCostRangeAsync(decimal minCost, decimal maxCost);
        Task<IEnumerable<Destino>> SearchDestinosAsync(string searchTerm);
        Task<bool> ExistsByNameAndCountryAsync(string nombre, string pais);
    }
}
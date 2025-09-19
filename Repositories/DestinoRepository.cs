using Microsoft.EntityFrameworkCore;
using GestionViajes.API.Data;
using GestionViajes.API.Interfaces;
using GestionViajes.API.Models;

namespace GestionViajes.API.Repositories
{
    public class DestinoRepository : GenericRepository<Destino>, IDestinoRepository
    {
        public DestinoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Destino>> GetDestinosByPaisAsync(string pais)
        {
            return await _dbSet
                .Where(d => d.Pais.ToLower().Contains(pais.ToLower()))
                .OrderBy(d => d.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Destino>> GetDestinosByCostRangeAsync(decimal minCost, decimal maxCost)
        {
            return await _dbSet
                .Where(d => d.Costo >= minCost && d.Costo <= maxCost)
                .OrderBy(d => d.Costo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Destino>> SearchDestinosAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var term = searchTerm.ToLower();
            return await _dbSet
                .Where(d => d.Nombre.ToLower().Contains(term) ||
                           d.Pais.ToLower().Contains(term) ||
                           (d.Descripcion != null && d.Descripcion.ToLower().Contains(term)))
                .OrderBy(d => d.Nombre)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAndCountryAsync(string nombre, string pais)
        {
            return await _dbSet
                .AnyAsync(d => d.Nombre.ToLower() == nombre.ToLower() && 
                              d.Pais.ToLower() == pais.ToLower());
        }
    }
}
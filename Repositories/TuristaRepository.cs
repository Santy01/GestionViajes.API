using Microsoft.EntityFrameworkCore;
using GestionViajes.API.Data;
using GestionViajes.API.Interfaces;
using GestionViajes.API.Models;

namespace GestionViajes.API.Repositories
{
    public class TuristaRepository : GenericRepository<Turista>, ITuristaRepository
    {
        public TuristaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Turista?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<Turista>> SearchTuristasAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var term = searchTerm.ToLower();
            return await _dbSet
                .Where(t => t.Nombre.ToLower().Contains(term) ||
                           t.Apellido.ToLower().Contains(term) ||
                           t.Email.ToLower().Contains(term) ||
                           (t.Telefono != null && t.Telefono.Contains(searchTerm)))
                .OrderBy(t => t.Apellido)
                .ThenBy(t => t.Nombre)
                .ToListAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet
                .AnyAsync(t => t.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> ExistsByEmailAsync(string email, int excludeId)
        {
            return await _dbSet
                .AnyAsync(t => t.Email.ToLower() == email.ToLower() && t.TuristaId != excludeId);
        }
    }
}
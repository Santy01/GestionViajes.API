using Microsoft.EntityFrameworkCore;
using GestionViajes.API.Data;
using GestionViajes.API.Interfaces;
using GestionViajes.API.Models;

namespace GestionViajes.API.Repositories
{
    public class ReservaRepository : GenericRepository<Reserva>, IReservaRepository
    {
        public ReservaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Reserva>> GetByTuristaAsync(int turistaId)
        {
            return await _dbSet
                .Where(r => r.TuristaId == turistaId)
                .OrderByDescending(r => r.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetByDestinoAsync(int destinoId)
        {
            return await _dbSet
                .Where(r => r.DestinoId == destinoId)
                .OrderByDescending(r => r.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetByDateRangeAsync(DateTime desde, DateTime hasta)
        {
            return await _dbSet
                .Where(r => r.FechaInicio >= desde && r.FechaFin <= hasta)
                .OrderBy(r => r.FechaInicio)
                .ToListAsync();
        }

        public async Task<bool> ExistsOverlappingAsync(int turistaId, DateTime inicio, DateTime fin, int? excludeReservaId = null)
        {
            return await _dbSet.AnyAsync(r =>
                r.TuristaId == turistaId &&
                (excludeReservaId == null || r.ReservaId != excludeReservaId) &&
                r.FechaInicio < fin && r.FechaFin > inicio);
        }
    }
}
using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiMetasAnalistas.Repositories
{
    public class OccurrenceRepository : Repository<Occurrence>, IOccurrenceRepository
    {
        public OccurrenceRepository(AppDBContext context) : base(context)
        {
        }
        //TODO: Finalizar alteração dos repositórios de Região e Ticket
        public override async Task<IEnumerable<Occurrence>> GetAllAsync()
        {
            return await _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .ToListAsync();
        }

        public override async Task<Occurrence?> GetReadOnlyAsync(Expression<Func<Occurrence, bool>> predicate)
        {
            return await _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .FirstOrDefaultAsync(predicate);
        }

        public override async Task<Occurrence?> GetAsync(Expression<Func<Occurrence, bool>> predicate)
        {
            return await _context.Occurrences
                .Include(a => a.Analista)
                .FirstOrDefaultAsync(predicate);
        }
        public async Task<IEnumerable<Occurrence>> GetByAnalystAsync(int analystId)
        {
            return await _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .Where(o => o.AnalistaId == analystId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Occurrence>> GetByAnalystPeriodAsync(int analystId, DateTime startDate, DateTime endDate)
        {
            return await _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .Where(o => o.AnalistaId == analystId
                       && o.DataInicio <= endDate
                       && o.DataFim >= startDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Occurrence>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .Where(o => o.DataInicio <= endDate && o.DataFim >= startDate)
                .ToListAsync();
        }

        public async Task<bool> HasOcurrencesAsync(int id, DateTime occurrenceDate)
        {
            return await _context.Occurrences
                .Where(o => o.AnalistaId == id
                    && o.DataInicio <= occurrenceDate
                    && o.DataFim >= occurrenceDate)
                .AnyAsync(o => o.AnalistaId == id);
        }
    }
}

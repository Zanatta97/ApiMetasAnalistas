using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiMetasAnalistas.Repositories
{
    public class AnalystRepository : Repository<Analyst>, IAnalystRepository
    {
        //private readonly AppDBContext _context;

        public AnalystRepository(AppDBContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Analyst>> GetAllAsync()
        {
            return await _context.Analysts
                .AsNoTracking()
                .Include(a => a.Regiao)
                .ToListAsync();
        }

        public override async Task<Analyst?> GetAsync(Expression<Func<Analyst, bool>> predicate)
        {
            return await _context.Analysts
                .Include(a => a.Regiao)
                .FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Retorna o analista sem Tracking
        /// Melhora o desempenho quando não há necessidade de alteração do objeto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<Analyst?> GetReadOnlyAsync(Expression<Func<Analyst, bool>> predicate)
        {
            return _context.Analysts
                .AsNoTracking()
                .Include(a => a.Regiao)
                .FirstOrDefault(predicate);
        }

        public async Task<Analyst?> GetByUserNameAsync(string userName)
        {
            return await _context.Analysts
                .AsNoTracking()
                .Include(a => a.Regiao)
                .FirstOrDefaultAsync(a => a.Usuario == userName);
        }

        public async Task<bool> HasOccurrencesAsync(int id)
        {
            return await _context.Occurrences.AnyAsync(o => o.AnalistaId == id);
        }

        public async Task<bool> HasTicketsAsync(int id)
        {
            return await _context.Tickets.AnyAsync(t => t.AnalystId == id);
        }

        public async Task<bool> IsHolidayAsync(Analyst analyst, DateTime currentDate)
        {
            return await _context.Holidays
                    .Where(h => h.RegiaoId == analyst.RegiaoId || h.RegiaoId == 1) //Ambiente Nacional
                    .AnyAsync(h => h.Data.Date == currentDate);
        }

        public async Task<int> TicketCountAsync(int id, DateTime startDate, DateTime endDate)
        {
            return await _context.Tickets
                        .Where(t => t.AnalystId == id && t.DataFechamento.Date >= startDate.Date && t.DataFechamento.Date <= endDate.Date)
                        .CountAsync();
        }

        public async Task<bool> HasOccurrenceAsync(int id, DateTime occurrenceDate)
        {
            return await _context.Occurrences.Where(d => d.DataInicio >= occurrenceDate && d.DataFim <= occurrenceDate).AnyAsync(o => o.AnalistaId == id);
        }


    }
}

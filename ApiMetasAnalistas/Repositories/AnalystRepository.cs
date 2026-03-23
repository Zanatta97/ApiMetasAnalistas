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

        public override IEnumerable<Analyst> GetAll()
        {
            return _context.Analysts
                .AsNoTracking()
                .Include(a => a.Regiao)
                .ToList();
        }

        public override Analyst? Get(Expression<Func<Analyst, bool>> predicate)
        {
            return _context.Analysts
                .Include(a => a.Regiao)
                .FirstOrDefault(predicate);
        }

        /// <summary>
        /// Retorna o analista sem Tracking
        /// Melhora o desempenho quando não há necessidade de alteração do objeto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Analyst? GetReadOnly(Expression<Func<Analyst, bool>> predicate)
        {
            return _context.Analysts
                .AsNoTracking()
                .Include(a => a.Regiao)
                .FirstOrDefault(predicate);
        }

        public Analyst? GetByUserName(string userName)
        {
            return _context.Analysts
                .AsNoTracking()
                .Include(a => a.Regiao)
                .FirstOrDefault(a => a.Usuario == userName);
        }

        public bool HasOccurrences(int id)
        {
            return _context.Occurrences.Any(o => o.AnalistaId == id);
        }

        public bool HasTickets(int id)
        {
            return _context.Tickets.Any(t => t.AnalystId == id);
        }

        public bool IsHoliday(Analyst analyst, DateTime currentDate)
        {
            return _context.Holidays
                    .Where(h => h.RegiaoId == analyst.RegiaoId || h.RegiaoId == 1) //Ambiente Nacional
                    .Any(h => h.Data.Date == currentDate);
        }

        public int TicketCount(int id, DateTime startDate, DateTime endDate)
        {
            return _context.Tickets
                        .Where(t => t.AnalystId == id && t.DataFechamento.Date >= startDate.Date && t.DataFechamento.Date <= endDate.Date)
                        .Count();
        }

        public bool HasOccurrence(int id, DateTime occurrenceDate)
        {
            return _context.Occurrences.Where(d => d.DataInicio >= occurrenceDate && d.DataFim <= occurrenceDate).Any(o => o.AnalistaId == id);
        }


    }
}

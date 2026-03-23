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
        public override IEnumerable<Occurrence> GetAll()
        {
            return _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .ToList();
        }

        public override Occurrence? GetReadOnly(Expression<Func<Occurrence, bool>> predicate)
        {
            return _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .FirstOrDefault(predicate);
        }

        public override Occurrence? Get(Expression<Func<Occurrence, bool>> predicate)
        {
            return _context.Occurrences
                .Include(a => a.Analista)
                .FirstOrDefault(predicate);
        }
        public IEnumerable<Occurrence> GetByAnalyst(int analystId)
        {
            return _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .Where(o => o.AnalistaId == analystId)
                .ToList();
        }

        public IEnumerable<Occurrence> GetByAnalystPeriod(int analystId, DateTime startDate, DateTime endDate)
        {
            return _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .Where(o => o.AnalistaId == analystId
                       && o.DataInicio <= endDate
                       && o.DataFim >= startDate)
                .ToList();
        }

        public IEnumerable<Occurrence> GetByPeriod(DateTime startDate, DateTime endDate)
        {
            return _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .Where(o => o.DataInicio <= endDate && o.DataFim >= startDate)
                .ToList();
        }

        public bool HasOcurrences(int id, DateTime occurrenceDate)
        {
            return _context.Occurrences
                .Where(o => o.AnalistaId == id
                    && o.DataInicio <= occurrenceDate
                    && o.DataFim >= occurrenceDate)
                .Any(o => o.AnalistaId == id);
        }
    }
}

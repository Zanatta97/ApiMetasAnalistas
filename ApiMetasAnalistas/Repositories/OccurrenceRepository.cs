using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Repositories
{
    public class OccurrenceRepository : IOccurrenceRepository
    {
        private readonly AppDBContext _context;

        public OccurrenceRepository(AppDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Occurrence> GetAll()
        {
            return _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .ToList();
        }

        public Occurrence? GetReadOnly(int id)
        {
            return _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .FirstOrDefault(o => o.Id == id);
        }

        public Occurrence? Get(int id)
        {
            return _context.Occurrences
                .Include(a => a.Analista)
                .FirstOrDefault(o => o.Id == id);
        }

        public void Add(Occurrence occurrence)
        {
            _context.Occurrences.Add(occurrence);
            _context.SaveChanges();
        }

        public void Update(Occurrence occurrence)
        {
            _context.Occurrences.Update(occurrence);
            _context.SaveChanges();
        }

        public void Delete(Occurrence occurrence)
        {
            _context.Occurrences.Remove(occurrence);
            _context.SaveChanges();
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

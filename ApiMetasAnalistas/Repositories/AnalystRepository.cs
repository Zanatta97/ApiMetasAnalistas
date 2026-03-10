using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Repositories
{
    public class AnalystRepository : IAnalystRepository
    {
        private readonly AppDBContext _context;

        public AnalystRepository(AppDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Analyst> GetAll()
        {
            return _context.Analysts
                .AsNoTracking()
                .Include(a => a.Regiao)
                .ToList();
        }

        public Analyst? Get(int id)
        {
            return _context.Analysts
                .Include(a => a.Regiao)
                .FirstOrDefault(a => a.Id == id);
        }

        /// <summary>
        /// Retorna o analista sem Tracking
        /// Melhora o desempenho quando não há necessidade de alteração do objeto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Analyst? GetReadOnly(int id)
        {
            return _context.Analysts
                .AsNoTracking()
                .Include(a => a.Regiao)
                .FirstOrDefault(a => a.Id == id);
        }

        public Analyst? GetByUserName(string userName)
        {
            return _context.Analysts
                .AsNoTracking()
                .Include(a => a.Regiao)
                .FirstOrDefault(a => a.Usuario == userName);
        }

        public void Add(Analyst analyst)
        {
            _context.Analysts.Add(analyst);
            _context.SaveChanges();

        }

        public void Update(Analyst analyst)
        {            
            _context.Analysts.Update(analyst);
            _context.SaveChanges();
        }

        public void Delete(Analyst analyst)
        {
            _context.Analysts.Remove(analyst);
            _context.SaveChanges();
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

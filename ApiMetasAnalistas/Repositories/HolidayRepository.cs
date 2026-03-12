using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly AppDBContext _context;

        public HolidayRepository(AppDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Holiday> GetAll()
        {
            return _context.Holidays.AsNoTracking().Include(a => a.Regiao).ToList();
        }

        public Holiday? Get(int id)
        {
            return _context.Holidays.Include(a => a.Regiao).FirstOrDefault(a => a.Id == id);
        }

        public Holiday? GetReadOnly(int id)
        {
            return _context.Holidays.AsNoTracking().Include(a => a.Regiao).FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Holiday> GetByDate(DateTime data)
        {
            return _context.Holidays
                .AsNoTracking()
                .Include(a => a.Regiao)
                .Where(h => h.Data.Date == data.Date)
                .ToList();
        }

        public IEnumerable<Holiday> GetByPeriod(DateTime startDate, DateTime endDate)
        {
            return _context.Holidays
                .AsNoTracking()
                .Include(a => a.Regiao)
                .Where(h => h.Data.Date >= startDate && h.Data.Date <= endDate)
                .ToList();
        }

        public IEnumerable<Holiday> GetByRegion(int regionId, DateTime data)
        {
            return _context.Holidays
                .AsNoTracking()
                .Include(a => a.Regiao)
                .Where(h => h.Data.Date == data.Date && h.RegiaoId == regionId)
                .ToList();
        }

        public void Add(Holiday holiday)
        {
            _context.Holidays.Add(holiday);
            _context.SaveChanges();
        }
        public void Update(Holiday holiday)
        {
            _context.Holidays.Update(holiday);
            _context.SaveChanges();
        }

        public void Delete(Holiday holiday)
        {
            _context.Holidays.Remove(holiday);
            _context.SaveChanges();
        }
    }
}

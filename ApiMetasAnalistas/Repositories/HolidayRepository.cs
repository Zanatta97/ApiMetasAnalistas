using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiMetasAnalistas.Repositories
{
    public class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(AppDBContext context) : base(context)
        {
        }

        public override IEnumerable<Holiday> GetAll()
        {
            return _context.Holidays.AsNoTracking().Include(a => a.Regiao).ToList();
        }

        public override Holiday? Get(Expression<Func<Holiday, bool>> predicate)
        {
            return _context.Holidays.Include(a => a.Regiao).FirstOrDefault(predicate);
        }

        public override Holiday? GetReadOnly(Expression<Func<Holiday, bool>> predicate)
        {
            return _context.Holidays.AsNoTracking().Include(a => a.Regiao).FirstOrDefault(predicate);
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
    }
}

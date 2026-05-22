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

        public override async Task<IEnumerable<Holiday>> GetAllAsync()
        {
            return await _context.Holidays.AsNoTracking().Include(a => a.Regiao).ToListAsync();
        }

        public override async Task<Holiday?> GetAsync(Expression<Func<Holiday, bool>> predicate)
        {
            return await _context.Holidays.Include(a => a.Regiao).FirstOrDefaultAsync(predicate);
        }

        public override async Task<Holiday?> GetReadOnlyAsync(Expression<Func<Holiday, bool>> predicate)
        {
            return await _context.Holidays.AsNoTracking().Include(a => a.Regiao).FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Holiday>> GetByDateAsync(DateTime data)
        {
            return await _context.Holidays
                .AsNoTracking()
                .Include(a => a.Regiao)
                .Where(h => h.Data.Date == data.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Holiday>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Holidays
                .AsNoTracking()
                .Include(a => a.Regiao)
                .Where(h => h.Data.Date >= startDate && h.Data.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Holiday>> GetByRegionAsync(int regionId, DateTime data)
        {
            return await _context.Holidays
                .AsNoTracking()
                .Include(a => a.Regiao)
                .Where(h => h.Data.Date == data.Date && h.RegiaoId == regionId)
                .ToListAsync();
        }
    }
}

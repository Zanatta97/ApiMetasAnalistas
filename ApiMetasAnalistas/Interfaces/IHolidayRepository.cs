using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IHolidayRepository : IRepository<Holiday>
    {
        IEnumerable<Holiday> GetByDate(DateTime data);
        IEnumerable<Holiday> GetByRegion(int regionId, DateTime data);
        IEnumerable<Holiday> GetByPeriod(DateTime startDate, DateTime endDate);
    }
}

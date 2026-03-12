using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IHolidayService
    {
        IEnumerable<Holiday> GetAll();
        Holiday? Get(int id);
        Holiday? GetReadOnly(int id);
        IEnumerable<Holiday> GetByDate(DateTime data);
        IEnumerable<Holiday> GetByRegion(int regionId, DateTime data);
        IEnumerable<Holiday> GetByPeriod(DateTime startDate, DateTime endDate);
        Holiday Add(Holiday holiday);
        Holiday Update(int id, Holiday holiday);
        void Delete(int id);
    }
}

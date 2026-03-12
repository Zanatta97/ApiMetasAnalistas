using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IHolidayRepository
    {
        IEnumerable<Holiday> GetAll();
        Holiday? Get(int id);
        Holiday? GetReadOnly(int id);
        IEnumerable<Holiday> GetByDate(DateTime data);
        IEnumerable<Holiday> GetByRegion(int regionId, DateTime data);
        IEnumerable<Holiday> GetByPeriod(DateTime startDate, DateTime endDate);
        void Add(Holiday holiday);
        void Update(Holiday holiday);
        void Delete(Holiday holiday);
    }
}

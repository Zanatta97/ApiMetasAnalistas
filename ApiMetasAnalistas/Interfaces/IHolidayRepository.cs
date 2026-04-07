using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IHolidayRepository : IRepository<Holiday>
    {
        Task<IEnumerable<Holiday>> GetByDateAsync(DateTime data);
        Task<IEnumerable<Holiday>> GetByRegionAsync(int regionId, DateTime data);
        Task<IEnumerable<Holiday>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
    }
}

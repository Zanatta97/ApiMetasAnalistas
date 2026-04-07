using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IHolidayService
    {
        Task<IEnumerable<Holiday>> GetAllAsync();
        Task<Holiday?> GetAsync(int id);
        Task<Holiday?> GetReadOnlyAsync(int id);
        Task<IEnumerable<Holiday>> GetByDateAsync(DateTime data);
        Task<IEnumerable<Holiday>> GetByRegionAsync(int regionId, DateTime data);
        Task<IEnumerable<Holiday>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
        Task<Holiday> AddAsync(Holiday holiday);
        Task<Holiday> UpdateAsync(int id, Holiday holiday);
        Task DeleteAsync(int id);
    }
}

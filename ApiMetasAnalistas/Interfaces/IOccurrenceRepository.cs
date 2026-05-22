using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IOccurrenceRepository : IRepository<Occurrence>
    {
        public Task<IEnumerable<Occurrence>> GetByAnalystAsync(int analystId);
        public Task<IEnumerable<Occurrence>> GetByAnalystPeriodAsync(int analystId, DateTime startDate, DateTime endDate);
        public Task<IEnumerable<Occurrence>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
        public Task<bool> HasOcurrencesAsync(int id, DateTime occurrenceDate);

    }
}

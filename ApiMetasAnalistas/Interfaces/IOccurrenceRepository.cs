using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IOccurrenceRepository : IRepository<Occurrence>
    {
        public IEnumerable<Occurrence> GetByAnalyst(int analystId);
        public IEnumerable<Occurrence> GetByAnalystPeriod(int analystId, DateTime startDate, DateTime endDate);
        public IEnumerable<Occurrence> GetByPeriod(DateTime startDate, DateTime endDate);
        public bool HasOcurrences(int id, DateTime occurrenceDate);

    }
}

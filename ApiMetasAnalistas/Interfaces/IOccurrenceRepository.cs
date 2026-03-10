using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IOccurrenceRepository
    {
        public IEnumerable<Occurrence> GetAll();
        public Occurrence? Get(int id);
        public Occurrence? GetReadOnly(int id);
        public void Add(Occurrence occurrence);
        public void Update(Occurrence occurrence);
        public void Delete(Occurrence occurrence);
        public IEnumerable<Occurrence> GetByAnalyst(int analystId);
        public IEnumerable<Occurrence> GetByAnalystPeriod(int analystId, DateTime startDate, DateTime endDate);
        public IEnumerable<Occurrence> GetByPeriod(DateTime startDate, DateTime endDate);
        public bool HasOcurrences(int id, DateTime occurrenceDate);

    }
}

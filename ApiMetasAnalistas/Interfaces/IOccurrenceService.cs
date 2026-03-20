using ApiMetasAnalistas.Enums;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IOccurrenceService
    {
        public IEnumerable<Occurrence> GetAll();
        public Occurrence? Get(int id);
        public Occurrence? GetReadOnly(int id);
        public Occurrence Add(Occurrence occurrence);
        public Occurrence Update(int id, Occurrence occurrence);
        public void Delete(int id);
        public IEnumerable<Occurrence> GetByAnalyst(int analystId);
        public IEnumerable<Occurrence> GetByAnalystPeriod(int analystId, DateTime startDate, DateTime endDate);
        public IEnumerable<Occurrence> GetByPeriod(DateTime startDate, DateTime endDate);
        public bool HasOcurrences(int id, DateTime occurrenceDate);
    }
}

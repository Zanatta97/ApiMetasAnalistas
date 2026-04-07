using ApiMetasAnalistas.Enums;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IOccurrenceService
    {
        public Task<IEnumerable<Occurrence>> GetAllAsync();
        public Task<Occurrence?> GetAsync(int id);
        public Task<Occurrence?> GetReadOnlyAsync(int id);
        public Task<Occurrence> AddAsync(Occurrence occurrence);
        public Task<Occurrence> UpdateAsync(int id, Occurrence occurrence);
        public Task DeleteAsync(int id);
        public Task<IEnumerable<Occurrence>> GetByAnalystAsync(int analystId);
        public Task<IEnumerable<Occurrence>> GetByAnalystPeriodAsync(int analystId, DateTime startDate, DateTime endDate);
        public Task<IEnumerable<Occurrence>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
        public Task<bool> HasOcurrencesAsync(int id, DateTime occurrenceDate);
    }
}

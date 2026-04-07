using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IAnalystRepository : IRepository<Analyst>
    {
        public Task<Analyst?> GetByUserNameAsync(string userName);
        public Task<bool> HasOccurrencesAsync(int id);
        public Task<bool> HasTicketsAsync(int id);
        public Task<bool> IsHolidayAsync(Analyst analyst, DateTime currentDate);
        public Task<int> TicketCountAsync(int id, DateTime startDate, DateTime endDate);
        public Task<bool> HasOccurrenceAsync(int id, DateTime occurrenceDate);
    }
}

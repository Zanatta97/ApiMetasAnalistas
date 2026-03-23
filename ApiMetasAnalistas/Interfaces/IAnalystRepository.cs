using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IAnalystRepository : IRepository<Analyst>
    {
        public Analyst? GetByUserName(string userName);
        public bool HasOccurrences(int id);
        public bool HasTickets(int id);
        public bool IsHoliday(Analyst analyst, DateTime currentDate);
        public int TicketCount(int id, DateTime startDate, DateTime endDate);
        public bool HasOccurrence(int id, DateTime occurrenceDate);
    }
}

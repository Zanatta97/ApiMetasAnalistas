using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IAnalystRepository
    {
        public IEnumerable<Analyst> GetAll();
        public Analyst? Get(int id);

        /// <summary>
        /// Retorna o analista sem Tracking
        /// Melhora o desempenho quando não há necessidade de alteração do objeto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Analyst? GetReadOnly(int id);
        public Analyst? GetByUserName(string userName);
        public void Add(Analyst analyst);
        public void Update(Analyst analyst);
        public void Delete(Analyst analyst);
        public bool HasOcurrences(int id);
        public bool HasTickets(int id);
        public bool IsHoliday(Analyst analyst, DateTime currentDate);
        public int TicketCount(int id, DateTime startDate, DateTime endDate);
    }
}

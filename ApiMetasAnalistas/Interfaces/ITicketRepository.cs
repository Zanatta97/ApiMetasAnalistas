using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        IEnumerable<Ticket> GetByAnalystId(int analystId);
    }
}

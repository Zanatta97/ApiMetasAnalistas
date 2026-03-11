using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> GetAll();
        Ticket? Get(int id);
        Ticket? GetReadOnly(int id);
        IEnumerable<Ticket> GetByAnalystId(int analystId);
        void Add(Ticket ticket);
        void Update(Ticket ticket);
        void Delete(Ticket ticket);
    }
}

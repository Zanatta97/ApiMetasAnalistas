using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface ITicketService
    {
        IEnumerable<Ticket> GetAll();
        Ticket? Get(int id);
        Ticket? GetReadOnly(int id);
        IEnumerable<Ticket> GetByAnalystId(int analystId);
        Ticket Add(Ticket ticket);
        Ticket Update(int id, Ticket ticket);
        void Delete(int id);
    }
}

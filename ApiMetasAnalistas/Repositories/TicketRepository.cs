using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Repositories
{
    public class TicketRepository : Repository<Ticket>,  ITicketRepository
    {

        public TicketRepository(AppDBContext context) : base(context)
        {
        }
        public IEnumerable<Ticket> GetByAnalystId(int analystId)
        {
            return _context.Tickets.Where(t => t.AnalystId == analystId).ToList();
        }
    }
}

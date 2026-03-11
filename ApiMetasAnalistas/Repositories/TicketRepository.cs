using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDBContext _context;

        public TicketRepository(AppDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _context.Tickets.ToList();
        }

        public Ticket? Get(int id)
        {
            return _context.Tickets.FirstOrDefault(t => t.Id == id);
        }

        public Ticket? GetReadOnly(int id)
        {
            return _context.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<Ticket> GetByAnalystId(int analystId)
        {
            return _context.Tickets.Where(t => t.AnalystId == analystId).ToList();
        }

        public void Add(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }

        public void Update(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            _context.SaveChanges();
        }

        public void Delete(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
            _context.SaveChanges();
        }
    }
}

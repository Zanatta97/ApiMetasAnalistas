using ApiMetasAnalistas.Common.Pagination;
using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace ApiMetasAnalistas.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {

        public TicketRepository(AppDBContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Ticket>> GetByAnalystIdAsync(int analystId)
        {
            return await _context.Tickets.Where(t => t.AnalystId == analystId).ToListAsync();
        }

        public async Task<IPagedList<Ticket>> GetPagedAsync(PaginationParameters parameters)
        {
            var tickets = await GetAllAsync();

            tickets = tickets.OrderBy(t => t.Id).AsQueryable();

            var resultado = tickets.ToPagedList(parameters.PageNumber, parameters.PageSize);

            return resultado;
        }

        public async Task<IPagedList<Ticket>> GetFilteredAsync(string filter, PaginationParameters parameters)
        {
            var analystId = await _context.Analysts.Where(a => a.Nome.Contains(filter) || a.Usuario.Contains(filter))
                            .Select(a => a.Id)
                            .ToListAsync();

            if (!analystId.Any())
            {
                return new List<Ticket>().ToPagedList(parameters.PageNumber, parameters.PageSize);
            }

            var tickets = _context.Tickets.Where(t => analystId.Contains(t.AnalystId)).OrderBy(t => t.Id).AsQueryable();

            var resultado = tickets.ToPagedList(parameters.PageNumber, parameters.PageSize);

            return resultado;
        }
    }
}

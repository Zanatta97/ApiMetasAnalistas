using ApiMetasAnalistas.Common.Pagination;
using ApiMetasAnalistas.Models;
using X.PagedList;

namespace ApiMetasAnalistas.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetByAnalystIdAsync(int analystId);
        Task<IPagedList<Ticket>> GetPagedAsync(PaginationParameters parameters);
        Task<IPagedList<Ticket>> GetFilteredAsync(string filter, PaginationParameters parameters);
    }
}

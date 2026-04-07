using ApiMetasAnalistas.Common.Pagination;
using ApiMetasAnalistas.Models;
using X.PagedList;

namespace ApiMetasAnalistas.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<Ticket?> GetAsync(int id);
        Task<Ticket?> GetReadOnlyAsync(int id);
        Task<IEnumerable<Ticket>> GetByAnalystIdAsync(int analystId);
        Task<IPagedList<Ticket>> GetPagedAsync(PaginationParameters parameters);
        Task<IPagedList<Ticket>> GetFilteredAsync(string filter, PaginationParameters parameters);
        Task<Ticket> AddAsync(Ticket ticket);
        Task<Ticket> UpdateAsync(int id, Ticket ticket);
        Task DeleteAsync(int id);
    }
}

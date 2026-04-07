using ApiMetasAnalistas.Common.Pagination;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ApiMetasAnalistas.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnityOfWork _repository;

        public TicketService(IUnityOfWork repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _repository.TicketRepository.GetAllAsync();
        }

        public async Task<Ticket?> GetAsync(int id)
        {
            return await _repository.TicketRepository.GetAsync(t => t.Id == id);
        }

        public async Task<Ticket?> GetReadOnlyAsync(int id)
        {
            return await _repository.TicketRepository.GetReadOnlyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Ticket>> GetByAnalystIdAsync(int analystId)
        {
            return await _repository.TicketRepository.GetByAnalystIdAsync(analystId);
        }

        public async Task<Ticket> AddAsync(Ticket ticket)
        {
            ArgumentNullException.ThrowIfNull(ticket);

            if (ticket.AnalystId <= 0)
            {
                throw new ArgumentException($"Valor {ticket.AnalystId} inválido para o Id do analista", nameof(ticket.AnalystId));
            }
            if (ticket.DataFechamento == default || ticket.DataFechamento > DateTime.Now)
            {
                throw new ArgumentException($"Valor {ticket.DataFechamento} inválido para a data de fechamento", nameof(ticket.DataFechamento));
            }

            try
            {
                _repository.TicketRepository.Add(ticket);
                await _repository.Commit();
                return ticket;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao adicionar a ocorrência", e);
            }
        }

        public async Task<Ticket> UpdateAsync(int id, Ticket ticket)
        {
            ArgumentNullException.ThrowIfNull(ticket);

            var existingTicket = await _repository.TicketRepository.GetAsync(t => t.Id == id);

            if (existingTicket == null)
            {
                throw new KeyNotFoundException($"Ticket com id {id} não encontrado.");
            }

            try
            {
                existingTicket.AnalystId = ticket.AnalystId;
                existingTicket.DataFechamento = ticket.DataFechamento;

                _repository.TicketRepository.Update(existingTicket);
                await _repository.Commit();
                return existingTicket;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao atualizar o ticket", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingTicket = await _repository.TicketRepository.GetAsync(t => t.Id == id);

            if (existingTicket == null)
            {
                throw new KeyNotFoundException($"Ticket com id {id} não encontrado.");
            }
            try
            {
                _repository.TicketRepository.Delete(existingTicket);
                await _repository.Commit();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao excluir o ticket", e);
            }
        }

        public async Task<IPagedList<Ticket>> GetPagedAsync(PaginationParameters parameters)
        {
            var tickets = await _repository.TicketRepository.GetPagedAsync(parameters);

            return tickets;
        }

        public async Task<IPagedList<Ticket>> GetFilteredAsync(string filter, PaginationParameters parameters)
        {
            return await _repository.TicketRepository.GetFilteredAsync(filter, parameters);
        }
    }
}

using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;

        public TicketService(ITicketRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _repository.GetAll();
        }

        public Ticket? Get(int id)
        {
            return _repository.Get(id);
        }

        public Ticket? GetReadOnly(int id)
        {
            return _repository.GetReadOnly(id);
        }

        public IEnumerable<Ticket> GetByAnalystId(int analystId)
        {
            return _repository.GetByAnalystId(analystId);
        }

        public Ticket Add(Ticket ticket)
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
                _repository.Add(ticket);
                return ticket;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao adicionar a ocorrência", e);
            }
        }

        public Ticket Update(int id, Ticket ticket)
        {
            ArgumentNullException.ThrowIfNull(ticket);

            var existingTicket = _repository.Get(id);

            if (existingTicket == null)
            {
                throw new KeyNotFoundException($"Ticket com id {id} não encontrado.");
            }

            try
            {
                existingTicket.AnalystId = ticket.AnalystId;
                existingTicket.DataFechamento = ticket.DataFechamento;

                _repository.Update(existingTicket);
                return existingTicket;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao atualizar o ticket", e);
            }
        }

        public void Delete(int id)
        {
            var existingTicket = _repository.Get(id);

            if (existingTicket == null)
            {
                throw new KeyNotFoundException($"Ticket com id {id} não encontrado.");
            }
            try
            {
                _repository.Delete(existingTicket);
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao excluir o ticket", e);
            }
        }
    }
}

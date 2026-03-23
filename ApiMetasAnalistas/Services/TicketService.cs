using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnityOfWork _repository;

        public TicketService(IUnityOfWork repository)
        {
            _repository = repository;
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _repository.TicketRepository.GetAll();
        }

        public Ticket? Get(int id)
        {
            return _repository.TicketRepository.Get(t => t.Id == id);
        }

        public Ticket? GetReadOnly(int id)
        {
            return _repository.TicketRepository.GetReadOnly(t => t.Id == id);
        }

        public IEnumerable<Ticket> GetByAnalystId(int analystId)
        {
            return _repository.TicketRepository.GetByAnalystId(analystId);
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
                _repository.TicketRepository.Add(ticket);
                _repository.Commit();
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

            var existingTicket = _repository.TicketRepository.Get(t => t.Id == id);

            if (existingTicket == null)
            {
                throw new KeyNotFoundException($"Ticket com id {id} não encontrado.");
            }

            try
            {
                existingTicket.AnalystId = ticket.AnalystId;
                existingTicket.DataFechamento = ticket.DataFechamento;

                _repository.TicketRepository.Update(existingTicket);
                _repository.Commit();
                return existingTicket;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao atualizar o ticket", e);
            }
        }

        public void Delete(int id)
        {
            var existingTicket = _repository.TicketRepository.Get(t => t.Id == id);

            if (existingTicket == null)
            {
                throw new KeyNotFoundException($"Ticket com id {id} não encontrado.");
            }
            try
            {
                _repository.TicketRepository.Delete(existingTicket);
                _repository.Commit();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Ocorreu um erro ao excluir o ticket", e);
            }
        }
    }
}

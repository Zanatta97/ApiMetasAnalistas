using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.DTO;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status500InternalServerError)]
    public class TicketsController : ControllerBase
    {

        private readonly ITicketService _service;

        public TicketsController(ITicketService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ticket>> Get()
        {
            var tickets = _service.GetAll();

            if (!tickets.Any())
                throw new KeyNotFoundException("Nenhum ticket cadastrado no sistema");

            return Ok(tickets);
        }

        [HttpGet("{id:int}", Name = "GetTicket")]
        public ActionResult<Ticket> Get(int id)
        {
            var ticket = _service.GetReadOnly(id);

            if (ticket is null)
                throw new KeyNotFoundException("Ticket não encontrado");
            
            return Ok(ticket);
        }

        [HttpGet("analyst/{idAnalista:int}", Name = "GetTicketsByAnalyst")]
        public ActionResult<IEnumerable<Ticket>> GetByAnalyst(int idAnalista)
        {
            var tickets = _service.GetByAnalystId(idAnalista);

            if (!tickets.Any())
                throw new KeyNotFoundException("Nenhum chamado encontrado para o analista especificado");

            return Ok(tickets);
        }

        [HttpPost]
        public ActionResult Post(Ticket ticket)
        {
            if (ticket is null)
                throw new ArgumentNullException("Chamado inválido");

            var newTicket = _service.Add(ticket);

            return new CreatedAtRouteResult("GetTicket", new { id = newTicket.Id }, newTicket);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Ticket ticket)
        {
            if (ticket is null)
                throw new ArgumentNullException("Chamado inválido");

            if (id != ticket.Id)
                throw new ArgumentException("ID do chamado não corresponde ao ID fornecido na URL");
                
            return Ok(_service.Update(id, ticket));
        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok($"Ticket com ID {id} deletado com sucesso");
        }
    }
}

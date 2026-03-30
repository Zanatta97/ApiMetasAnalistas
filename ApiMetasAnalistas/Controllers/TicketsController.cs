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
        [ProducesResponseType(typeof(IEnumerable<TicketResponseDTO>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<TicketResponseDTO>> Get()
        {
            var tickets = _service.GetAll();

            if (!tickets.Any())
                throw new KeyNotFoundException("Nenhum ticket cadastrado no sistema");

            return Ok(tickets.ToDTOList());
        }

        [HttpGet("{id:int}", Name = "GetTicket")]
        [ProducesResponseType(typeof(TicketResponseDTO), StatusCodes.Status200OK)]
        public ActionResult<TicketResponseDTO> Get(int id)
        {
            var ticket = _service.GetReadOnly(id);

            if (ticket is null)
                throw new KeyNotFoundException("Ticket não encontrado");

            return Ok(ticket.ToDTO());
        }

        [HttpGet("analyst/{idAnalista:int}", Name = "GetTicketsByAnalyst")]
        [ProducesResponseType(typeof(IEnumerable<TicketResponseDTO>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<TicketResponseDTO>> GetByAnalyst(int idAnalista)
        {
            var tickets = _service.GetByAnalystId(idAnalista);

            if (!tickets.Any())
                throw new KeyNotFoundException("Nenhum chamado encontrado para o analista especificado");

            return Ok(tickets.ToDTOList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(TicketResponseDTO), StatusCodes.Status201Created)]
        public ActionResult<TicketResponseDTO> Post(TicketRequestDTO ticket)
        {
            if (ticket is null)
                throw new ArgumentNullException("Chamado inválido");

            var newTicket = _service.Add(ticket.ToEntity()!);

            return new CreatedAtRouteResult("GetTicket", new { id = newTicket.Id }, newTicket.ToDTO());
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(TicketResponseDTO), StatusCodes.Status200OK)]
        public ActionResult<TicketResponseDTO> Put(int id, TicketRequestDTO ticket)
        {
            if (ticket is null)
                throw new ArgumentNullException("Chamado inválido");
                
            return Ok(_service.Update(id, ticket.ToEntity()!).ToDTO());
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok($"Ticket com ID {id} deletado com sucesso");
        }
    }
}

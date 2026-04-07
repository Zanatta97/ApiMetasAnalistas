using ApiMetasAnalistas.Common.Pagination;
using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.DTO;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using X.PagedList;

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
        public async Task<ActionResult<IEnumerable<TicketResponseDTO>>> Get()
        {
            var tickets = await _service.GetAllAsync();

            if (!tickets.Any())
                throw new KeyNotFoundException("Nenhum ticket cadastrado no sistema");

            return Ok(tickets.ToDTOList());
        }

        [HttpGet("{id:int}", Name = "GetTicket")]
        [ProducesResponseType(typeof(TicketResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<TicketResponseDTO>> Get(int id)
        {
            var ticket = await _service.GetReadOnlyAsync(id);

            if (ticket is null)
                throw new KeyNotFoundException("Ticket não encontrado");

            return Ok(ticket.ToDTO());
        }

        [HttpGet("analyst/{idAnalista:int}", Name = "GetTicketsByAnalyst")]
        [ProducesResponseType(typeof(IEnumerable<TicketResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TicketResponseDTO>>> GetByAnalyst(int idAnalista)
        {
            var tickets = await _service.GetByAnalystIdAsync(idAnalista);

            if (!tickets.Any())
                throw new KeyNotFoundException("Nenhum chamado encontrado para o analista especificado");

            return Ok(tickets.ToDTOList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(TicketResponseDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<TicketResponseDTO>> Post(TicketRequestDTO ticket)
        {
            if (ticket is null)
                throw new ArgumentNullException("Chamado inválido");

            var newTicket = await _service.AddAsync(ticket.ToEntity()!);

            return new CreatedAtRouteResult("GetTicket", new { id = newTicket.Id }, newTicket.ToDTO());
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(TicketResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<TicketResponseDTO>> Put(int id, TicketRequestDTO ticket)
        {
            if (ticket is null)
                throw new ArgumentNullException("Chamado inválido");

            var updatedTicket = await _service.UpdateAsync(id, ticket.ToEntity()!);

            return Ok(updatedTicket.ToDTO());
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok($"Ticket com ID {id} deletado com sucesso");
        }

        [HttpGet("page")]
        [ProducesResponseType(typeof(IEnumerable<TicketResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TicketResponseDTO>>> GetPaged([FromQuery] PaginationParameters parameters)
        {
            var pagedTickets = await _service.GetPagedAsync(parameters);

            if (!pagedTickets.Any())
                throw new KeyNotFoundException("Nenhum ticket encontrado para os parâmetros de paginação especificados");

            AddPaginationMetadata(pagedTickets);

            return Ok(pagedTickets.ToDTOList());
        }

        [HttpGet("filter")]
        [ProducesResponseType(typeof(IEnumerable<TicketResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TicketResponseDTO>>> GetFiltered([FromQuery] string filter,
                                                                                    [FromQuery] PaginationParameters parameters)
        {
            var filteredTickets = await _service.GetFilteredAsync(filter, parameters);

            AddPaginationMetadata(filteredTickets);

            return Ok(filteredTickets.ToDTOList());
        }



        private void AddPaginationMetadata(IPagedList list)
        {
            var metadata = new
            {
                list.PageNumber,
                list.PageSize,
                list.PageCount,
                list.TotalItemCount,
                list.HasNextPage,
                list.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        }
    }
}

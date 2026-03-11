using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
                return NotFound("Nenhum ticket cadastrado no sistema");

            return Ok(tickets);
        }

        [HttpGet("{id:int}", Name = "GetTicket")]
        public ActionResult<Ticket> Get(int id)
        {
            var ticket = _service.GetReadOnly(id);

            if (ticket is null)
                return NotFound("Ticket não encontrado");
            
            return Ok(ticket);
        }

        [HttpPost]
        public ActionResult Post(Ticket ticket)
        {
            try
            {
                if (ticket is null)
                    return BadRequest("Chamado inválido");

                var newTicket = _service.Add(ticket);

                return new CreatedAtRouteResult("GetTicket", new { id = newTicket.Id }, newTicket);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(new { message = e.Message });
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new { message = "Erro no banco de dados", details = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro inesperado: {e.Message}");
            }
        }

        [HttpGet("analyst/{idAnalista:int}", Name = "GetTicketsByAnalyst")]
        public ActionResult<IEnumerable<Ticket>> GetByAnalyst(int idAnalista)
        {
            var tickets = _service.GetByAnalystId(idAnalista);

            if (!tickets.Any())
                return NotFound("Nenhum chamado encontrado para o analista especificado");
            
            return Ok(tickets);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Ticket ticket)
        {
            try
            {
                if (ticket is null)
                    return BadRequest("Chamado inválido");

                if (id != ticket.Id)
                    return BadRequest("ID do chamado não corresponde ao ID fornecido na URL");
                
                return Ok(_service.Update(id, ticket));
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new { message = "Erro no banco de dados", details = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar o analista de ID {id}: {e.Message}");
            }
        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);

                return Ok($"Ticket com ID {id} deletado com sucesso");
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(new { message = e.Message });
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new { message = "Erro no banco de dados", details = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Erro inesperado: {e.Message}");
            }
        }
    }
}

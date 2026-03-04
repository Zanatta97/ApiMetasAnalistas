using ApiMetasAnalistas.Context;
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

        private readonly AppDBContext _context;

        public TicketsController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ticket>> Get()
        {
            var tickets = _context.Tickets.AsNoTracking().ToList();

            if (tickets is null || tickets.Count == 0)
                return NotFound("Nenhum ticket cadastrado no sistema");

            return tickets;
        }

        [HttpGet("{id:int}", Name = "GetTicket")]
        public ActionResult<Ticket> Get(int id)
        {
            var ticket = _context.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == id);

            if (ticket is null)
                return NotFound("Ticket não encontrado");
            
            return ticket;
        }

        [HttpPost]
        public ActionResult Post(Ticket ticket)
        {
            try
            {
                if (ticket is null)
                    return BadRequest("Chamado inválido");

                _context.Tickets.Add(ticket);
                _context.SaveChanges();
                
                return new CreatedAtRouteResult("GetTicket", new { id = ticket.Id }, ticket);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao inserir o chamado: {e.Message}");
            }
        }

        [HttpGet("analyst/{idAnalista:int}", Name = "GetTicketsByAnalyst")]
        public ActionResult<IEnumerable<Ticket>> GetByAnalyst(int idAnalista)
        {
            var tickets = _context.Tickets
                .AsNoTracking()
                .Where(t => t.AnalystId == idAnalista)
                .ToList();

            if (tickets is null || tickets.Count == 0)
                return NotFound("Nenhum chamado encontrado para o analista especificado");
            
            return tickets;
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Ticket ticket)
        {
            try
            {
                if (ticket is null || ticket.Id != id)
                    return BadRequest("Chamado inválido");

                var existingTicket = _context.Tickets.FirstOrDefault(t => t.Id == id);
                
                if (existingTicket is null)
                    return NotFound("Chamado não encontrado");
                
                existingTicket.AnalystId = ticket.AnalystId;
                existingTicket.DataFechamento = ticket.DataFechamento;

                _context.Tickets.Update(existingTicket);
                _context.SaveChanges();
                
                return Ok(existingTicket);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar o chamado: {e.Message}");
            }
        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
                if (ticket is null)
                    return NotFound("Chamado não encontrado");
                _context.Tickets.Remove(ticket);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao excluir o chamado: {e.Message}");
            }
        }




        }
}

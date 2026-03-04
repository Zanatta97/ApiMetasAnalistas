using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OccurrencesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public OccurrencesController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Occurrence>> Get()
        {
            var occurrences = _context.Occurrences.AsNoTracking().Include(a => a.Analista).ToList();

            if (occurrences is null || occurrences.Count == 0)
                return NotFound("Nenhuma ocorrência cadastrada no sistema");

            return occurrences;
        }

        [HttpGet("{id:int}", Name = "GetOccurrence")]
        public ActionResult<Occurrence> Get(int id)
        {
            var occurrence = _context.Occurrences.AsNoTracking().Include(a => a.Analista).FirstOrDefault(a => a.Id == id);

            if (occurrence is null)
                return NotFound("Ocorrência não encontrada");

            return occurrence;
        }

        [HttpGet("analyst/{idAnalista:int}", Name = "GetByAnalyst")]
        public ActionResult<IEnumerable<Occurrence>> GetByAnalyst(int idAnalista)
        {
            var occurrences = _context.Occurrences
                .AsNoTracking()
                .Include(a => a.Analista)
                .Where(o => o.AnalistaId == idAnalista)
                .ToList();

            if (occurrences is null || occurrences.Count == 0)
                return NotFound("Nenhuma ocorrência encontrada para o analista especificado");
            
            return occurrences;
        }

        [HttpPost]
        public ActionResult Post(Occurrence occurrence)
        {
            try
            {
                if (occurrence is null)
                    return BadRequest("Ocorrência inválida");
                _context.Occurrences.Add(occurrence);
                _context.SaveChanges();
                return new CreatedAtRouteResult("GetOccurrence", new { id = occurrence.Id }, occurrence);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao inserir a ocorrência: {e.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Occurrence occurrence)
        {
            try
            {
                if (id != occurrence.Id)
                    return BadRequest("ID da ocorrência não corresponde ao ID do recurso");

                var existingOccurrence = _context.Occurrences.AsNoTracking().FirstOrDefault(o => o.Id == id);
                
                if (existingOccurrence is null)
                    return NotFound("Ocorrência não encontrada");
                
                _context.Occurrences.Update(occurrence);
                _context.SaveChanges();
                
                return Ok(existingOccurrence);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar a ocorrência: {e.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var occurrence = _context.Occurrences.FirstOrDefault(o => o.Id == id);

                if (occurrence is null)
                    return NotFound("Ocorrência não encontrada");

                _context.Occurrences.Remove(occurrence);
                _context.SaveChanges();

                return Ok($"Ocorrência com ID {id} excluída com sucesso");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao excluir a ocorrência: {e.Message}");
            }
        }
    }
}

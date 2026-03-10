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
    public class OccurrencesController : ControllerBase
    {
        private readonly IOccurrenceService _service;

        public OccurrencesController(IOccurrenceService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Occurrence>> Get()
        {
            try
            {
                var occurrences = _service.GetAll();

                if (!occurrences.Any())
                    return NotFound("Nenhuma ocorrência cadastrada no sistema");

                return Ok(occurrences);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar as ocorrências: {e.Message}");
            }
        }

        [HttpGet("{id:int}", Name = "GetOccurrence")]
        public ActionResult<Occurrence> Get(int id)
        {
            var occurrence = _service.Occurrences.AsNoTracking().Include(a => a.Analista).FirstOrDefault(a => a.Id == id);

            if (occurrence is null)
                return NotFound("Ocorrência não encontrada");

            return occurrence;
        }

        [HttpGet("analyst/{idAnalista:int}", Name = "GetByAnalyst")]
        public ActionResult<IEnumerable<Occurrence>> GetByAnalyst(int idAnalista)
        {
            var occurrences = _service.Occurrences
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
                _service.Occurrences.Add(occurrence);
                _service.SaveChanges();
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

                var existingOccurrence = _service.Occurrences.AsNoTracking().FirstOrDefault(o => o.Id == id);
                
                if (existingOccurrence is null)
                    return NotFound("Ocorrência não encontrada");
                
                _service.Occurrences.Update(occurrence);
                _service.SaveChanges();
                
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
                var occurrence = _service.Occurrences.FirstOrDefault(o => o.Id == id);

                if (occurrence is null)
                    return NotFound("Ocorrência não encontrada");

                _service.Occurrences.Remove(occurrence);
                _service.SaveChanges();

                return Ok($"Ocorrência com ID {id} excluída com sucesso");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao excluir a ocorrência: {e.Message}");
            }
        }

        [HttpGet("period/")]
        public ActionResult<IEnumerable<Occurrence>> GetByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var occurrences = _service.Occurrences.AsNoTracking().Include(a => a.Analista).ToList();

            if (occurrences is null || occurrences.Count == 0)
                return NotFound("Nenhuma ocorrência cadastrada no sistema");

            return occurrences;
        }

        [HttpGet("period/{id:int}")]
        public ActionResult<Occurrence> GetByAnalystPeriod(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var occurrence = _service.Occurrences.AsNoTracking().Include(a => a.Analista).FirstOrDefault(a => a.Id == id);

            if (occurrence is null)
                return NotFound("Ocorrência não encontrada");

            return occurrence;
        }
    }
}

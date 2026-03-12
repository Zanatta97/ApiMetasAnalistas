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
            var occurrence = _service.GetReadOnly(id);

            if (occurrence is null)
                return NotFound("Ocorrência não encontrada");

            return Ok(occurrence);
        }

        [HttpGet("analyst/{idAnalista:int}", Name = "GetByAnalyst")]
        public ActionResult<IEnumerable<Occurrence>> GetByAnalyst(int idAnalista)
        {
            var occurrences = _service.GetByAnalyst(idAnalista);

            if (!occurrences.Any())
                return NotFound("Nenhuma ocorrência encontrada para o analista especificado");

            return Ok(occurrences);
        }

        [HttpPost]
        public ActionResult Post(Occurrence occurrence)
        {
            try
            {
                if (occurrence is null)
                    return BadRequest("Ocorrência inválida");

                var newOcurrence = _service.Add(occurrence);

                return new CreatedAtRouteResult("GetOccurrence", new { id = newOcurrence.Id }, newOcurrence);
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

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Occurrence occurrence)
        {
            try
            {
                if (occurrence is null)
                    return BadRequest("Ocorrência inválida");

                if (id != occurrence.Id)
                    return BadRequest("ID da ocorrência não corresponde ao ID do recurso");

                return Ok(_service.Update(id, occurrence));
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar a ocorrencia de ID {id}: {e.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);

                return Ok($"Ocorrência com ID {id} excluída com sucesso");
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

        [HttpGet("period/")]
        public ActionResult<IEnumerable<Occurrence>> GetByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var occurrences = _service.GetByPeriod(startDate, endDate);

            if (!occurrences.Any())
                return NotFound("Nenhuma ocorrência cadastrada no período");

            return Ok(occurrences);
        }

        [HttpGet("period/{id:int}")]
        public ActionResult<IEnumerable<Occurrence>> GetByAnalystPeriod(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var occurrences = _service.GetByAnalystPeriod(id, startDate, endDate);

            if (!occurrences.Any())
                return NotFound("Ocorrência não encontrada");

            return Ok(occurrences);
        }

        [HttpGet("hasOccurrence/{id:int}")]
        public ActionResult<bool> HasOcurrences(int id, [FromQuery] DateTime occurrenceDate)
        {
            var hasOcurrences = _service.HasOcurrences(id, occurrenceDate);
            return Ok(hasOcurrences);
        }
    }
}

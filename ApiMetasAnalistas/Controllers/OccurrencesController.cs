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
            var occurrences = _service.GetAll();

            if (!occurrences.Any())
                throw new KeyNotFoundException("Nenhuma ocorrência cadastrada no sistema");

            return Ok(occurrences);
        }

        [HttpGet("{id:int}", Name = "GetOccurrence")]
        public ActionResult<Occurrence> Get(int id)
        {
            var occurrence = _service.GetReadOnly(id);

            if (occurrence is null)
                throw new KeyNotFoundException("Ocorrência não encontrada");

            return Ok(occurrence);
        }

        [HttpGet("analyst/{idAnalista:int}", Name = "GetByAnalyst")]
        public ActionResult<IEnumerable<Occurrence>> GetByAnalyst(int idAnalista)
        {
            var occurrences = _service.GetByAnalyst(idAnalista);

            if (!occurrences.Any())
                throw new KeyNotFoundException("Nenhuma ocorrência encontrada para o analista especificado");

            return Ok(occurrences);
        }

        [HttpPost]
        public ActionResult Post(Occurrence occurrence)
        {
            if (occurrence is null)
                throw new ArgumentNullException("Ocorrência inválida");

            var newOcurrence = _service.Add(occurrence);

            return new CreatedAtRouteResult("GetOccurrence", new { id = newOcurrence.Id }, newOcurrence);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Occurrence occurrence)
        {
            if (occurrence is null)
                throw new ArgumentNullException("Ocorrência inválida");

            if (id != occurrence.Id)
                throw new ArgumentException("ID da ocorrência não corresponde ao ID do recurso");

            return Ok(_service.Update(id, occurrence));
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok($"Ocorrência com ID {id} excluída com sucesso");
        }

        [HttpGet("period/")]
        public ActionResult<IEnumerable<Occurrence>> GetByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var occurrences = _service.GetByPeriod(startDate, endDate);

            if (!occurrences.Any())
                throw new KeyNotFoundException("Nenhuma ocorrência cadastrada no período");

            return Ok(occurrences);
        }

        [HttpGet("period/{id:int}")]
        public ActionResult<IEnumerable<Occurrence>> GetByAnalystPeriod(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var occurrences = _service.GetByAnalystPeriod(id, startDate, endDate);

            if (!occurrences.Any())
                throw new KeyNotFoundException("Ocorrência não encontrada");

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

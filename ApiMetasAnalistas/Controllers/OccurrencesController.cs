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
        [ProducesResponseType(typeof(IEnumerable<OccurrenceResponseDTO>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OccurrenceResponseDTO>> Get()
        {
            var occurrences = _service.GetAll();

            if (!occurrences.Any())
                throw new KeyNotFoundException("Nenhuma ocorrência cadastrada no sistema");

            return Ok(occurrences.ToDTOList());
        }

        [HttpGet("{id:int}", Name = "GetOccurrence")]
        [ProducesResponseType(typeof(OccurrenceResponseDTO), StatusCodes.Status200OK)]
        public ActionResult<OccurrenceResponseDTO> Get(int id)
        {
            var occurrence = _service.GetReadOnly(id);

            if (occurrence is null)
                throw new KeyNotFoundException("Ocorrência não encontrada");

            return Ok(occurrence.ToDTO());
        }

        [HttpGet("analyst/{idAnalista:int}", Name = "GetByAnalyst")]
        [ProducesResponseType(typeof(IEnumerable<OccurrenceResponseDTO>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OccurrenceResponseDTO>> GetByAnalyst(int idAnalista)
        {
            var occurrences = _service.GetByAnalyst(idAnalista);

            if (!occurrences.Any())
                throw new KeyNotFoundException("Nenhuma ocorrência encontrada para o analista especificado");

            return Ok(occurrences.ToDTOList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(OccurrenceResponseDTO), StatusCodes.Status201Created)]
        public ActionResult<OccurrenceResponseDTO> Post(OccurrenceRequestDTO occurrence)
        {
            if (occurrence is null)
                throw new ArgumentNullException("Ocorrência inválida");

            var newOcurrence = _service.Add(occurrence.ToEntity()!);

            return new CreatedAtRouteResult("GetOccurrence", new { id = newOcurrence.Id }, newOcurrence.ToDTO());
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(OccurrenceResponseDTO), StatusCodes.Status200OK)]
        public ActionResult<OccurrenceResponseDTO> Put(int id, OccurrenceRequestDTO occurrence)
        {
            if (occurrence is null)
                throw new ArgumentNullException("Ocorrência inválida");

            return Ok(_service.Update(id, occurrence.ToEntity()!).ToDTO());
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok($"Ocorrência com ID {id} excluída com sucesso");
        }

        [HttpGet("period/")]
        public ActionResult<IEnumerable<OccurrenceResponseDTO>> GetByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var occurrences = _service.GetByPeriod(startDate, endDate);

            if (!occurrences.Any())
                throw new KeyNotFoundException("Nenhuma ocorrência cadastrada no período");

            return Ok(occurrences.ToDTOList());
        }

        [HttpGet("period/{id:int}")]
        [ProducesResponseType(typeof(IEnumerable<OccurrenceResponseDTO>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OccurrenceResponseDTO>> GetByAnalystPeriod(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var occurrences = _service.GetByAnalystPeriod(id, startDate, endDate);

            if (!occurrences.Any())
                throw new KeyNotFoundException("Ocorrência não encontrada");

            return Ok(occurrences.ToDTOList());
        }

        [HttpGet("hasOccurrence/{id:int}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult<bool> HasOcurrences(int id, [FromQuery] DateTime occurrenceDate)
        {
            var hasOcurrences = _service.HasOcurrences(id, occurrenceDate);
            return Ok(hasOcurrences);
        }
    }
}

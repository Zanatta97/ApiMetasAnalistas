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
        public async Task<ActionResult<IEnumerable<OccurrenceResponseDTO>>> Get()
        {
            var occurrences = await _service.GetAllAsync();

            if (!occurrences.Any())
                throw new KeyNotFoundException("Nenhuma ocorrência cadastrada no sistema");

            return Ok(occurrences.ToDTOList());
        }

        [HttpGet("{id:int}", Name = "GetOccurrence")]
        [ProducesResponseType(typeof(OccurrenceResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<OccurrenceResponseDTO>> Get(int id)
        {
            var occurrence = await _service.GetReadOnlyAsync(id);

            if (occurrence is null)
                throw new KeyNotFoundException("Ocorrência não encontrada");

            return Ok(occurrence.ToDTO());
        }

        [HttpGet("analyst/{idAnalista:int}", Name = "GetByAnalyst")]
        [ProducesResponseType(typeof(IEnumerable<OccurrenceResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OccurrenceResponseDTO>>> GetByAnalyst(int idAnalista)
        {
            var occurrences = await _service.GetByAnalystAsync(idAnalista);

            if (!occurrences.Any())
                throw new KeyNotFoundException("Nenhuma ocorrência encontrada para o analista especificado");

            return Ok(occurrences.ToDTOList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(OccurrenceResponseDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<OccurrenceResponseDTO>> Post(OccurrenceRequestDTO occurrence)
        {
            if (occurrence is null)
                throw new ArgumentNullException("Ocorrência inválida");

            var newOcurrence = await _service.AddAsync(occurrence.ToEntity()!);

            return new CreatedAtRouteResult("GetOccurrence", new { id = newOcurrence.Id }, newOcurrence.ToDTO());
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(OccurrenceResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<OccurrenceResponseDTO>> Put(int id, OccurrenceRequestDTO occurrence)
        {
            if (occurrence is null)
                throw new ArgumentNullException("Ocorrência inválida");

            var updatedOccurrence = await _service.UpdateAsync(id, occurrence.ToEntity()!);

            return Ok(updatedOccurrence.ToDTO());
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok($"Ocorrência com ID {id} excluída com sucesso");
        }

        [HttpGet("period/")]
        public async Task<ActionResult<IEnumerable<OccurrenceResponseDTO>>> GetByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var occurrences = await _service.GetByPeriodAsync(startDate, endDate);

            if (!occurrences.Any())
                throw new KeyNotFoundException("Nenhuma ocorrência cadastrada no período");

            return Ok(occurrences.ToDTOList());
        }

        [HttpGet("period/{id:int}")]
        [ProducesResponseType(typeof(IEnumerable<OccurrenceResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OccurrenceResponseDTO>>> GetByAnalystPeriod(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var occurrences = await _service.GetByAnalystPeriodAsync(id, startDate, endDate);

            if (!occurrences.Any())
                throw new KeyNotFoundException("Ocorrência não encontrada");

            return Ok(occurrences.ToDTOList());
        }

        [HttpGet("hasOccurrence/{id:int}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> HasOcurrences(int id, [FromQuery] DateTime occurrenceDate)
        {
            var hasOcurrences = await _service.HasOcurrencesAsync(id, occurrenceDate);
            return Ok(hasOcurrences);
        }
    }
}

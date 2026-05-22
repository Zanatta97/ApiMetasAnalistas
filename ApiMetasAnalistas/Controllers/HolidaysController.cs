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
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayService _service;

        public HolidaysController(IHolidayService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HolidayResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HolidayResponseDTO>>> Get()
        {
            var holidays = await _service.GetAllAsync();

            if (holidays is null)
                throw new KeyNotFoundException("Nenhum feriado cadastrado no sistema");

            return Ok(holidays.ToDTOList());
        }

        [HttpGet("{id:int}", Name = "GetHoliday")]
        [ProducesResponseType(typeof(HolidayResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<HolidayResponseDTO>> Get(int id)
        {
            var holiday = await _service.GetReadOnlyAsync(id);

            if (holiday is null)
                throw new KeyNotFoundException("Feriado não encontrado");

            return Ok(holiday.ToDTO());
        }

        [HttpGet("{data:datetime}", Name = "GetHolidayByDate")]
        [ProducesResponseType(typeof(IEnumerable<HolidayResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HolidayResponseDTO>>> Get(DateTime data)
        {
            var holidays = await _service.GetByDateAsync(data);

            if (!holidays.Any())
                throw new KeyNotFoundException("Nenhum feriado encontrado para a data especificada");

            return Ok(holidays.ToDTOList());
        }

        [HttpGet("region/{regionId:int}")]
        [ProducesResponseType(typeof(IEnumerable<HolidayResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HolidayResponseDTO>>> GetByRegion(int regionId, [FromQuery] DateTime date)
        {
            var holidays = await _service.GetByRegionAsync(regionId, date);

            if (!holidays.Any())
                throw new KeyNotFoundException("Nenhum feriado encontrado para o período especificado");

            return Ok(holidays.ToDTOList());
        }

        [HttpGet("period")]
        [ProducesResponseType(typeof(IEnumerable<HolidayResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<HolidayResponseDTO>>> GetByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var holidays = await _service.GetByPeriodAsync(startDate, endDate);

            if (!holidays.Any())
                throw new KeyNotFoundException("Nenhum feriado encontrado para o período especificado");

            return Ok(holidays.ToDTOList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(HolidayResponseDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<HolidayResponseDTO>> Post(HolidayRequestDTO holiday)
        {
            if (holiday is null)
                throw new ArgumentNullException("Feriado inválido");
                
            var newHoliday = await _service.AddAsync(holiday.ToEntity()!);

            return new CreatedAtRouteResult("GetHoliday", new { id = newHoliday.Id }, newHoliday.ToDTO());
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(HolidayResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<HolidayResponseDTO>> Put(int id, HolidayRequestDTO holiday)
        {
            if (holiday is null)
                throw new ArgumentNullException("Feriado inválido");

            var updatedHoliday = await _service.UpdateAsync(id, holiday.ToEntity()!);

            return Ok(updatedHoliday.ToDTO());
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok($"Feriado de ID {id} deletado com sucesso");
        }


    }
}

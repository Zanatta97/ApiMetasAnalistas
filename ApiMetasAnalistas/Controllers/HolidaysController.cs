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
        public ActionResult<IEnumerable<HolidayResponseDTO>> Get()
        {
            var holidays = _service.GetAll();

            if (holidays is null)
                throw new KeyNotFoundException("Nenhum feriado cadastrado no sistema");

            return Ok(holidays.ToDTOList());
        }

        [HttpGet("{id:int}", Name = "GetHoliday")]
        public ActionResult<HolidayResponseDTO> Get(int id)
        {
            var holiday = _service.GetReadOnly(id);

            if (holiday is null)
                throw new KeyNotFoundException("Feriado não encontrado");

            return Ok(holiday.ToDTO());
        }

        [HttpGet("{data:datetime}", Name = "GetHolidayByDate")]
        public ActionResult<IEnumerable<HolidayResponseDTO>> Get(DateTime data)
        {
            var holidays = _service.GetByDate(data);

            if (!holidays.Any())
                throw new KeyNotFoundException("Nenhum feriado encontrado para a data especificada");

            return Ok(holidays.ToDTOList());
        }

        [HttpGet("region/{regionId:int}")]
        public ActionResult<IEnumerable<HolidayResponseDTO>> GetByRegion(int regionId, [FromQuery] DateTime date)
        {
            var holidays = _service.GetByRegion(regionId, date);

            if (!holidays.Any())
                throw new KeyNotFoundException("Nenhum feriado encontrado para o período especificado");

            return Ok(holidays.ToDTOList());
        }

        [HttpGet("period")]
        public ActionResult<IEnumerable<HolidayResponseDTO>> GetByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var holidays = _service.GetByPeriod(startDate, endDate);

            if (!holidays.Any())
                throw new KeyNotFoundException("Nenhum feriado encontrado para o período especificado");

            return Ok(holidays.ToDTOList());
        }

        [HttpPost]
        public ActionResult<HolidayResponseDTO> Post(HolidayRequestDTO holiday)
        {
            if (holiday is null)
                throw new ArgumentNullException("Feriado inválido");
                
            var newHoliday = _service.Add(holiday.ToEntity()!);

            return new CreatedAtRouteResult("GetHoliday", new { id = newHoliday.Id }, newHoliday.ToDTO());
        }

        [HttpPut("{id:int}")]
        public ActionResult<HolidayResponseDTO> Put(int id, HolidayRequestDTO holiday)
        {
            if (holiday is null)
                throw new ArgumentNullException("Feriado inválido");

            return Ok(_service.Update(id, holiday.ToEntity()!).ToDTO());
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok($"Feriado de ID {id} deletado com sucesso");
        }


    }
}

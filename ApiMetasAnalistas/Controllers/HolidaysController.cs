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
        public ActionResult<IEnumerable<Holiday>> Get()
        {
            var holidays = _service.GetAll();

            if (holidays is null)
                throw new KeyNotFoundException("Nenhum feriado cadastrado no sistema");

            return Ok(holidays);
        }

        [HttpGet("{id:int}", Name = "GetHoliday")]
        public ActionResult<Holiday> Get(int id)
        {
            var holiday = _service.GetReadOnly(id);

            if (holiday is null)
                throw new KeyNotFoundException("Feriado não encontrado");

            return Ok(holiday);
        }

        [HttpGet("{data:datetime}", Name = "GetHolidayByDate")]
        public ActionResult<IEnumerable<Holiday>> Get(DateTime data)
        {
            var holidays = _service.GetByDate(data);

            if (!holidays.Any())
                throw new KeyNotFoundException("Nenhum feriado encontrado para a data especificada");

            return Ok(holidays);
        }

        [HttpGet("region/{regionId:int}")]
        public ActionResult<IEnumerable<Holiday>> GetByRegion(int regionId, [FromQuery] DateTime date)
        {
            var holidays = _service.GetByRegion(regionId, date);

            if (!holidays.Any())
                throw new KeyNotFoundException("Nenhum feriado encontrado para o período especificado");

            return Ok(holidays);
        }

        [HttpGet("period")]
        public ActionResult<IEnumerable<Holiday>> GetByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var holidays = _service.GetByPeriod(startDate, endDate);

            if (!holidays.Any())
                throw new KeyNotFoundException("Nenhum feriado encontrado para o período especificado");

            return Ok(holidays);
        }

        [HttpPost]
        public ActionResult Post(Holiday holiday)
        {
            if (holiday is null)
                throw new ArgumentNullException("Feriado inválido");
                
            var newHoliday = _service.Add(holiday);

            return new CreatedAtRouteResult("GetHoliday", new { id = newHoliday.Id }, newHoliday);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Holiday holiday)
        {
            if (holiday is null)
                throw new ArgumentNullException("Feriado inválido");

            if (id != holiday.Id)
                throw new ArithmeticException("ID do feriado não corresponde ao ID da URL");

            return Ok(_service.Update(id, holiday));
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok($"Feriado de ID {id} deletado com sucesso");
        }


    }
}

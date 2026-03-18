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
                return NotFound("Nenhum feriado cadastrado no sistema");

            return Ok(holidays);
        }

        [HttpGet("{id:int}", Name = "GetHoliday")]
        public ActionResult<Holiday> Get(int id)
        {
            var holiday = _service.GetReadOnly(id);

            if (holiday is null)
                return NotFound("Feriado não encontrado");

            return Ok(holiday);
        }

        [HttpGet("{data:datetime}", Name = "GetHolidayByDate")]
        public ActionResult<IEnumerable<Holiday>> Get(DateTime data)
        {
            var holidays = _service.GetByDate(data);

            if (!holidays.Any())
                return NotFound("Nenhum feriado encontrado para a data especificada");

            return Ok(holidays);
        }

        [HttpGet("region/{regionId:int}")]
        public ActionResult<IEnumerable<Holiday>> GetByRegion(int regionId, [FromQuery] DateTime date)
        {
            var holidays = _service.GetByRegion(regionId, date);

            if (!holidays.Any())
                return NotFound("Nenhum feriado encontrado para o período especificado");

            return Ok(holidays);
        }

        [HttpGet("period")]
        public ActionResult<IEnumerable<Holiday>> GetByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var holidays = _service.GetByPeriod(startDate, endDate);

            if (!holidays.Any())
                return NotFound("Nenhum feriado encontrado para o período especificado");

            return Ok(holidays);
        }

        [HttpPost]
        public ActionResult Post(Holiday holiday)
        {
            try
            {
                if (holiday is null)
                    return BadRequest("Feriado inválido");
                
                var newHoliday = _service.Add(holiday);

                return new CreatedAtRouteResult("GetHoliday", new { id = newHoliday.Id }, newHoliday);
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
        public ActionResult Put(int id, Holiday holiday)
        {
            try
            {
                if (holiday is null)
                    return BadRequest("Feriado inválido");

                if (id != holiday.Id)
                    return BadRequest("ID do feriado não corresponde ao ID da URL");

                return Ok(_service.Update(id, holiday));
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar o Feriado de ID {id}: {e.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);

                return Ok($"Feriado de ID {id} deletado com sucesso");
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


    }
}

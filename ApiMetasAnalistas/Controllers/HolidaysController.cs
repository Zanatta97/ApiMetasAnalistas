using ApiMetasAnalistas.Context;
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
        private readonly AppDBContext _context;

        public HolidaysController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Holiday>> Get()
        {
            var holidays = _context.Holidays.AsNoTracking().Include(a => a.Regiao).ToList();

            if (holidays is null)
                return NotFound("Nenhum feriado cadastrado no sistema");

            return holidays;
        }

        [HttpGet("{id:int}", Name = "GetHoliday")]
        public ActionResult<Holiday> Get(int id)
        {
            var holiday = _context.Holidays.AsNoTracking().Include(a => a.Regiao).FirstOrDefault(a => a.Id == id);

            if (holiday is null)
                return NotFound("Feriado não encontrado");

            return holiday;
        }

        [HttpGet("{data:datetime}", Name = "GetHolidayByDate")]
        public ActionResult<IEnumerable<Holiday>> Get(DateTime data)
        {
            var holidays = _context.Holidays
                .AsNoTracking()
                .Include(a => a.Regiao)
                .Where(h => h.Data.Date == data.Date)
                .ToList();

            if (holidays is null || holidays.Count == 0)
                return NotFound("Nenhum feriado encontrado para a data especificada");

            return holidays;
        }

        [HttpPost]
        public ActionResult Post(Holiday holiday)
        {
            try
            {
                if (holiday is null)
                    return BadRequest("Feriado inválido");
                _context.Holidays.Add(holiday);
                _context.SaveChanges();
                return new CreatedAtRouteResult("GetHoliday", new { id = holiday.Id }, holiday);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao inserir o feriado: {e.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Holiday holiday)
        {
            try
            {
                if (id != holiday.Id)
                    return BadRequest("ID do feriado não corresponde ao ID da URL");

                var existingHoliday = _context.Holidays.FirstOrDefault(h => h.Id == id);

                if (existingHoliday is null)
                    return NotFound("Feriado não encontrado");

                existingHoliday.Data = holiday.Data;
                existingHoliday.Descricao = holiday.Descricao;
                existingHoliday.RegiaoId = holiday.RegiaoId;

                _context.Holidays.Update(existingHoliday);
                _context.SaveChanges();

                return Ok(existingHoliday);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar o feriado: {e.Message}");

            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var holiday = _context.Holidays.FirstOrDefault(h => h.Id == id);

                if (holiday is null)
                    return NotFound("Feriado não encontrado");
                
                _context.Holidays.Remove(holiday);
                _context.SaveChanges();
                
                return Ok($"Feriado de ID {id} deletado com sucesso");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar o feriado: {e.Message}");
            }
        }


    }
}

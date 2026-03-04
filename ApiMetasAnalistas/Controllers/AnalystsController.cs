using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.DTO;
using ApiMetasAnalistas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnalystsController : ControllerBase
    {

        private readonly AppDBContext _context;

        public AnalystsController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Analyst>> Get()
        {
            var analysts = _context.Analysts.AsNoTracking().Include(a => a.Regiao).ToList();

            if (analysts is null)
                return NotFound("Nenhum analista cadastrado no sistema");

            return analysts;
        }

        [HttpGet("{id:int}", Name = "GetAnalyst")]
        public ActionResult<Analyst> Get(int id)
        {
            var analyst = _context.Analysts.AsNoTracking().Include(a => a.Regiao).FirstOrDefault(a => a.Id == id);

            if (analyst is null)
                return NotFound("Analista não encontrado");
            return analyst;
        }

        [HttpPost]
        public ActionResult Post(Analyst analyst)
        {
            try
            {
                if (analyst is null)
                    return BadRequest("Analista inválido");

                _context.Analysts.Add(analyst);
                _context.SaveChanges();

                return new CreatedAtRouteResult("GetAnalyst", new { id = analyst.Id }, analyst);
            }
            catch (Exception e)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao inserir o analista: {e.Message}");
            }
            
        }

        [HttpGet("target/{id:int}", Name = "GetAnalystTarget")]
        public ActionResult GetAnalystTarget(int id)
        {
            var analyst = _context.Analysts.AsNoTracking().FirstOrDefault(a => a.Id == id);
            if (analyst is null)
                return NotFound("Analista não encontrado");

            return Ok(new { analyst.MetaDiaria });

        }

        [HttpGet("target/{id:int}/period/")]
        public ActionResult GetAnalystTargetForPeriod(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var analyst = _context.Analysts.AsNoTracking().FirstOrDefault(a => a.Id == id);

            if (analyst is null)
                return NotFound("Analista não encontrado");



            var totalDays = 0;//(endDate.Date - startDate.Date).TotalDays + 1;

            for (int i = 0; startDate.Date.AddDays(i) <= endDate.Date; i++)
            {
                var currentDate = startDate.Date.AddDays(i);

                var isHoliday = _context.Holidays
                    .Where(h => h.RegiaoId == analyst.RegiaoId || h.RegiaoId == 1)
                    .Any(h => h.Data.Date == currentDate);

                var isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;
                
                if (!isHoliday && !isWeekend)
                    totalDays++;
            }

            var targetForPeriod = analyst.MetaDiaria * totalDays;

            return Ok(new { TargetForPeriod = targetForPeriod });
        }

        [HttpGet("target")]
        public ActionResult GetTargetResults([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {

            try
            {
                var analysts = _context.Analysts.AsNoTracking().Include(a => a.Regiao).ToList();

                var targetResults = new List<AnalystResultDTO>();

                foreach (var analyst in analysts)
                {
                    var totalDays = 0;

                    for (int i = 0; startDate.Date.AddDays(i) <= endDate.Date; i++)
                    {
                        var currentDate = startDate.Date.AddDays(i);

                        var isHoliday = _context.Holidays
                            .Where(h => h.RegiaoId == analyst.RegiaoId || h.RegiaoId == 1)
                            .Any(h => h.Data.Date == currentDate);

                        var hasOccurrence = _context.Occurrences
                            .Where(o => o.AnalistaId == analyst.Id)
                            .Any(o => o.DataInicio.Date <= currentDate && o.DataFim >= currentDate);

                        var isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;

                        if (!isHoliday && !isWeekend && !hasOccurrence)
                            totalDays++;
                    }

                    var ticketsFechados = _context.Tickets
                            .Where(t => t.AnalystId == analyst.Id && t.DataFechamento.Date >= startDate.Date && t.DataFechamento.Date <= endDate.Date)
                            .Count();

                    targetResults.Add(new AnalystResultDTO
                    {
                        AnalistaId = analyst.Id,
                        NomeAnalista = analyst.Nome,
                        RegiaoId = analyst.RegiaoId,
                        TotalDiasUteis = totalDays,
                        MetaDiaria = analyst.MetaDiaria,
                        TotalMetaPeriodo = analyst.MetaDiaria * totalDays,
                        TicketsFechados = ticketsFechados,
                        PercentualMetaAlcancada = totalDays > 0 ? (decimal)ticketsFechados / (analyst.MetaDiaria * totalDays) * 100 : 0

                    });

                }


                return Ok(targetResults);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao calcular os resultados dos analistas: {e.Message}");
            }
        }



        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Analyst analyst)
        {
            //Outra forma de fazer isto seria utilizando o EntityState.Modified,
            //mas isso pode levar a problemas de segurança, como ataques de overposting,
            //onde um cliente mal-intencionado pode enviar dados adicionais que não deveriam ser atualizados.
            //Além disso, o uso do EntityState.Modified pode resultar em atualizações acidentais de campos que não foram
            //intencionalmente modificados, especialmente se o modelo tiver muitos campos ou relacionamentos complexos.
            //Portanto, é recomendado buscar o registro existente no banco de dados,
            //atualizar apenas os campos necessários e depois salvar as alterações para garantir um controle
            //mais preciso sobre o processo de atualização.
            /*if (id != analyst.Id)
                return BadRequest("ID do analista não corresponde ao ID fornecido na URL");

            _context.Entry(analyst).State = EntityState.Modified;
            _context.SaveChanges();*/

            try
            {
                if (analyst is null || analyst.Id != id)
                    return BadRequest("Analista inválido");

                var existingAnalyst = _context.Analysts.FirstOrDefault(a => a.Id == id);

                if (existingAnalyst is null)
                    return NotFound($"Analista de ID {id} não encontrado");

                existingAnalyst.Nome = analyst.Nome;
                existingAnalyst.Usuario = analyst.Usuario;
                existingAnalyst.RegiaoId = analyst.RegiaoId;
                existingAnalyst.MetaDiaria = analyst.MetaDiaria;

                _context.Analysts.Update(existingAnalyst);
                _context.SaveChanges();

                return Ok(existingAnalyst);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar o analista de ID {id}: {e.Message}");
            }
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var analyst = _context.Analysts.FirstOrDefault(a => a.Id == id);

                if (analyst is null)
                    return NotFound($"Analista de ID {id} não encontrado");

                _context.Analysts.Remove(analyst);
                _context.SaveChanges();

                return Ok($"Analista de ID {id} deletado com sucesso");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar o analista de ID {id}: {e.Message}");
            }
        }
    }
}

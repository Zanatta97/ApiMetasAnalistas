using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnalystsController : ControllerBase
    {

        private readonly IAnalystService _service;

        public AnalystsController(IAnalystService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Analyst>> Get()
        {
            try
            {
                var analysts = _service.GetAll();

                if (!analysts.Any())
                    return NotFound("Nenhum analista cadastrado no sistema");

                return Ok(analysts);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar analistas: {e.Message}");
            }
        }

        [HttpGet("{id:int}", Name = "GetAnalyst")]
        public ActionResult<Analyst> Get(int id)
        {
            try
            {
                var analyst = _service.Get(id);

                if (analyst is null)
                    return NotFound("Analista não encontrado");

                return Ok(analyst);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar o analista de ID {id}: {e.Message}");

            }
        }

        [HttpPost]
        public ActionResult Post(Analyst analyst)
        {
            try
            {
                if (analyst is null)
                    return BadRequest("Analista inválido");

                var newAnalyst = _service.Add(analyst);

                return new CreatedAtRouteResult("GetAnalyst", new { id = newAnalyst.Id }, newAnalyst);
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
        public ActionResult Put(int id, Analyst analyst)
        {
            try
            {
                if (analyst is null)
                    return BadRequest("Analista inválido");

                if (id != analyst.Id)
                    return BadRequest("ID do analista não corresponde ao ID informado no request");

                return Ok(_service.Update(id, analyst));
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar o analista de ID {id}: {e.Message}");
            }
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);

                return Ok($"Analista de ID {id} deletado com sucesso");
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

        [HttpGet("target/{id:int}", Name = "GetAnalystTarget")]
        public ActionResult<int> GetAnalystTarget(int id)
        {
            try
            {
                var analyst = _service.Get(id);

                if (analyst is null)
                    return NotFound("Analista não encontrado");

                return Ok(new { analyst.MetaDiaria });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao inserir o analista: {e.Message}");
            }

        }

        [HttpGet("target/{id:int}/period/")]
        public ActionResult<int> GetAnalystTargetForPeriod(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest("A data de início deve ser anterior à data de término");

                if (id <= 0 || startDate == default || endDate == default)
                    return BadRequest("Parâmetros inválidos");

                return Ok(new { Target = _service.GetTargetForPeriod(id, startDate, endDate) });
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao calcular a meta do analista de ID {id} para o período: {e.Message}");
            }
        }

        [HttpGet("target")]
        public ActionResult GetTargetResults([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {

            try
            {
                if (startDate > endDate)
                    return BadRequest("A data de início deve ser anterior à data de término");

                if (startDate == default || endDate == default)
                    return BadRequest("Parâmetros inválidos");

                return Ok(_service.GetTargetResults(startDate, endDate));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro inesperado ao calcular os resultados dos analistas: {e.Message}");
            }
        }

    }
}

using ApiMetasAnalistas.DTO;
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
        private readonly ILogger<AnalystsController> _logger;

        public AnalystsController(IAnalystService service, ILogger<AnalystsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Analyst>> Get()
        {
            var analysts = _service.GetAll();

            if (!analysts.Any())
                throw new KeyNotFoundException("Nenhum analista encontrado");

            return Ok(analysts);
        }

        [HttpGet("{id:int}", Name = "GetAnalyst")]
        public ActionResult<Analyst> Get(int id)
        {
            var analyst = _service.Get(id);

            if (analyst is null)
                throw new KeyNotFoundException("Nenhum analista encontrado");

            return Ok(analyst);
        }

        [HttpPost]
        public ActionResult Post(Analyst analyst)
        {
            if (analyst is null)
                throw new ArgumentNullException(nameof(analyst), "Analista inválido");

            var newAnalyst = _service.Add(analyst);

            return new CreatedAtRouteResult("GetAnalyst", new { id = newAnalyst.Id }, newAnalyst);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Analyst analyst)
        {
            if (analyst is null)
                throw new ArgumentNullException(nameof(analyst), "Analista inválido");

            if (id != analyst.Id)
                throw new ArgumentException("O ID do analista na URL deve corresponder ao ID no corpo da requisição");

            return Ok(_service.Update(id, analyst));         
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok($"Analista de ID {id} deletado com sucesso");
        }

        [HttpGet("target/{id:int}", Name = "GetAnalystTarget")]
        public ActionResult<AnalystResultDTO> GetAnalystTarget(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var analyst = _service.Get(id);

            if (analyst is null)
                throw new KeyNotFoundException("Nenhum analista encontrado");

            var targetResult = _service.GetAnalystTargetResults(startDate, endDate, analyst);

            return Ok(targetResult);
        }

        [HttpGet("target")]
        public ActionResult GetTargetResults([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("A data de início deve ser anterior à data de término");

            if (startDate == default || endDate == default)
                throw new ArgumentException("Data informada inválida");

            return Ok(_service.GetTargetResults(startDate, endDate));
        }

        [HttpGet("exists/{username}")]
        public ActionResult<bool> UsernameExists(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("O nome de usuário não pode ser vazio ou nulo", nameof(username));

            var analyst = _service.GetByUserName(username);

            return Ok(analyst is not null);
        }
    }
}

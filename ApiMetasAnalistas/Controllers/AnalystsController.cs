using ApiMetasAnalistas.DTO;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
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
        [ProducesResponseType(typeof(IEnumerable<AnalystResponseDTO>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<AnalystResponseDTO>> Get()
        {
            var analysts = _service.GetAll();

            if (!analysts.Any())
                throw new KeyNotFoundException("Nenhum analista encontrado");

            return Ok(analysts.ToDTOList());
        }

        [HttpGet("{id:int}", Name = "GetAnalyst")]
        [ProducesResponseType(typeof(AnalystResponseDTO), StatusCodes.Status200OK)]
        public ActionResult<AnalystResponseDTO> Get(int id)
        {
            var analyst = _service.Get(id);

            if (analyst is null)
                throw new KeyNotFoundException("Nenhum analista encontrado");

            return Ok(analyst.ToDTO());
        }

        [HttpPost]
        [ProducesResponseType(typeof(AnalystResponseDTO), StatusCodes.Status201Created)]
        public ActionResult<AnalystResponseDTO> Post(AnalystRequestDTO analyst)
        {
            if (analyst is null)
                throw new ArgumentNullException(nameof(analyst), "Analista inválido");

            var newAnalyst = _service.Add(analyst.ToEntity()!);

            return new CreatedAtRouteResult("GetAnalyst", new { id = newAnalyst.Id }, newAnalyst.ToDTO());
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(AnalystResponseDTO), StatusCodes.Status200OK)]
        public ActionResult<AnalystResponseDTO> Put(int id, AnalystRequestDTO analyst)
        {
            if (analyst is null)
                throw new ArgumentNullException(nameof(analyst), "Analista inválido");

            return Ok(_service.Update(id, analyst.ToEntity()!).ToDTO());         
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok($"Analista de ID {id} deletado com sucesso");
        }

        [HttpGet("target/{id:int}", Name = "GetAnalystTarget")]
        [ProducesResponseType(typeof(AnalystResultDTO), StatusCodes.Status200OK)]
        public ActionResult<AnalystResultDTO> GetAnalystTarget(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var analyst = _service.Get(id);

            if (analyst is null)
                throw new KeyNotFoundException("Nenhum analista encontrado");

            var targetResult = _service.GetAnalystTargetResults(startDate, endDate, analyst);

            return Ok(targetResult);
        }

        [HttpGet("target")]
        [ProducesResponseType(typeof(AnalystResultDTO), StatusCodes.Status200OK)]
        public ActionResult<AnalystResultDTO> GetTargetResults([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("A data de início deve ser anterior à data de término");

            if (startDate == default || endDate == default)
                throw new ArgumentException("Data informada inválida");

            return Ok(_service.GetTargetResults(startDate, endDate));
        }

        [HttpGet("exists/{username}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public ActionResult<bool> UsernameExists(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("O nome de usuário não pode ser vazio ou nulo", nameof(username));

            var analyst = _service.GetByUserName(username);

            return Ok(analyst is not null);
        }
    }
}

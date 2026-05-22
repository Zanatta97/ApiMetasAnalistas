using ApiMetasAnalistas.DTO;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

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
        public async Task<ActionResult<IEnumerable<AnalystResponseDTO>>> Get()
        {
            var analysts = await _service.GetAllAsync();

            if (!analysts.Any())
                throw new KeyNotFoundException("Nenhum analista encontrado");

            return Ok(analysts.ToDTOList());
        }

        [HttpGet("{id:int}", Name = "GetAnalyst")]
        [ProducesResponseType(typeof(AnalystResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<AnalystResponseDTO>> Get(int id)
        {
            var analyst = await _service.GetAsync(id);

            if (analyst is null)
                throw new KeyNotFoundException("Nenhum analista encontrado");

            return Ok(analyst.ToDTO());
        }

        [HttpPost]
        [ProducesResponseType(typeof(AnalystResponseDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<AnalystResponseDTO>> Post(AnalystRequestDTO analyst)
        {
            if (analyst is null)
                throw new ArgumentNullException(nameof(analyst), "Analista inválido");

            var newAnalyst = await _service.AddAsync(analyst.ToEntity()!);

            return new CreatedAtRouteResult("GetAnalyst", new { id = newAnalyst.Id }, newAnalyst.ToDTO());
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(AnalystResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<AnalystResponseDTO>> Put(int id, AnalystRequestDTO analyst)
        {
            if (analyst is null)
                throw new ArgumentNullException(nameof(analyst), "Analista inválido");

            var updatedAnalyst = await _service.UpdateAsync(id, analyst.ToEntity()!);

            return Ok(updatedAnalyst.ToDTO());         
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok($"Analista de ID {id} deletado com sucesso");
        }

        [HttpGet("target/{id:int}", Name = "GetAnalystTarget")]
        [ProducesResponseType(typeof(AnalystResultDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<AnalystResultDTO>> GetAnalystTarget(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var analyst = await _service.GetAsync(id);

            if (analyst is null)
                throw new KeyNotFoundException("Nenhum analista encontrado");

            var targetResult = await _service.GetAnalystTargetResultsAsync(startDate, endDate, analyst);

            return Ok(targetResult);
        }

        [HttpGet("target")]
        [ProducesResponseType(typeof(AnalystResultDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<AnalystResultDTO>> GetTargetResults([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("A data de início deve ser anterior à data de término");

            if (startDate == default || endDate == default)
                throw new ArgumentException("Data informada inválida");

            return Ok(await _service.GetTargetResultsAsync(startDate, endDate));
        }

        [HttpGet("exists/{username}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UsernameExists(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("O nome de usuário não pode ser vazio ou nulo", nameof(username));

            var analyst = await _service.GetByUserNameAsync(username);

            return Ok(analyst is not null);
        }
    }
}

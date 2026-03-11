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
    public class RegionsController : ControllerBase
    {
        private readonly IRegionService _service;

        public RegionsController(IRegionService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Region>> Get()
        {
            var regions = _service.GetAll();

            if (regions is null)
                return NotFound("Nenhuma região cadastrada no sistema");

            return Ok(regions);
        }

        [HttpGet("{id:int}", Name = "GetRegion")]
        public ActionResult<Region> Get(int id)
        {
            var region = _service.GetReadOnly(id);

            if (region is null)
                return NotFound("Região não encontrada");

            return region;
        }

        [HttpPost]
        public ActionResult Post(Region region)
        {
            try
            {
                if (region is null)
                    return BadRequest("Região inválida");

                var newRegion = _service.Add(region);

                return new CreatedAtRouteResult("GetRegion", new { id = newRegion.Id }, newRegion);
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
        public ActionResult Put(int id, Region region)
        {
            try
            {
                if (region is null)
                    return BadRequest("Região inválida");

                if (id != region.Id)
                    return BadRequest("ID da região não corresponde ao ID do recurso");

                return Ok(_service.Update(id, region));
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

                return Ok($"Região de ID {id} deletada com sucesso");
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

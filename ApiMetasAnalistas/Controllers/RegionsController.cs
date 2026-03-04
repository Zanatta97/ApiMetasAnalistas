using ApiMetasAnalistas.Context;
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
        private readonly AppDBContext _context;

        public RegionsController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Region>> Get()
        {
            var regions = _context.Regions.AsNoTracking().ToList();

            if (regions is null)
                return NotFound("Nenhuma região cadastrada no sistema");

            return regions;
        }

        [HttpGet("{id:int}", Name = "GetRegion")]
        public ActionResult<Region> Get(int id)
        {
            var region = _context.Regions.AsNoTracking().FirstOrDefault(r => r.Id == id);

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

                _context.Regions.Add(region);
                _context.SaveChanges();

                return new CreatedAtRouteResult("GetRegion", new { id = region.Id }, region);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao inserir a região: {e.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Region region)
        {
            try
            {
                if (id != region.Id)
                    return BadRequest("ID da região não corresponde ao ID do recurso");

                var existingRegion = _context.Regions.FirstOrDefault(r => r.Id == id);

                if (existingRegion is null)
                    return NotFound("Região não encontrada");

                existingRegion.Nome = region.Nome;

                _context.Regions.Update(existingRegion);
                _context.SaveChanges();

                return Ok(existingRegion);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao alterar a região: {e.Message}");
            }

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var region = _context.Regions.FirstOrDefault(r => r.Id == id);
                if (region is null)
                    return NotFound("Região não encontrada");
                _context.Regions.Remove(region);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar a região: {e.Message}");
            }


        }
    }
}

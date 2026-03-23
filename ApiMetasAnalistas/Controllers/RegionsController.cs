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
                throw new KeyNotFoundException("Nenhuma região cadastrada no sistema");

            return Ok(regions);
        }

        [HttpGet("{id:int}", Name = "GetRegion")]
        public ActionResult<Region> Get(int id)
        {
            var region = _service.GetReadOnly(id);

            if (region is null)
                throw new KeyNotFoundException("Região não encontrada");

            return region;
        }

        [HttpPost]
        public ActionResult Post(Region region)
        {
            if (region is null)
                throw new ArgumentNullException("Região inválida");

            var newRegion = _service.Add(region);

            return new CreatedAtRouteResult("GetRegion", new { id = newRegion.Id }, newRegion);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Region region)
        {
            if (region is null)
                throw new ArgumentNullException("Região inválida");

            if (id != region.Id)
                throw new ArgumentException("ID da região não corresponde ao ID do recurso");

            return Ok(_service.Update(id, region));
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok($"Região de ID {id} deletada com sucesso");
        }
    }
}

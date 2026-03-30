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
        [ProducesResponseType(typeof(IEnumerable<RegionResponseDTO>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<RegionResponseDTO>> Get()
        {
            var regions = _service.GetAll();

            if (regions is null)
                throw new KeyNotFoundException("Nenhuma região cadastrada no sistema");

            return Ok(regions.ToDTOList());
        }

        [HttpGet("{id:int}", Name = "GetRegion")]
        [ProducesResponseType(typeof(RegionResponseDTO), StatusCodes.Status200OK)]
        public ActionResult<RegionResponseDTO> Get(int id)
        {
            var region = _service.GetReadOnly(id);

            if (region is null)
                throw new KeyNotFoundException("Região não encontrada");

            return Ok(region.ToDTO());
        }

        [HttpPost]
        [ProducesResponseType(typeof(RegionResponseDTO), StatusCodes.Status201Created)]
        public ActionResult<RegionResponseDTO> Post(RegionRequestDTO region)
        {
            if (region is null)
                throw new ArgumentNullException("Região inválida");

            var newRegion = _service.Add(region.ToEntity()!);

            return new CreatedAtRouteResult("GetRegion", new { id = newRegion.Id }, newRegion.ToDTO());
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(RegionResponseDTO), StatusCodes.Status200OK)]
        public ActionResult<RegionResponseDTO> Put(int id, RegionRequestDTO region)
        {
            if (region is null)
                throw new ArgumentNullException("Região inválida");

            return Ok(_service.Update(id, region.ToEntity()!).ToDTO());
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);

            return Ok($"Região de ID {id} deletada com sucesso");
        }
    }
}

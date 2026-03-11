using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _repository;
        public RegionService(IRegionRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<Region> GetAll()
        {
            return _repository.GetAll();
        }
        public Region? Get(int id)
        {
            return _repository.Get(id);
        }
        public Region? GetReadOnly(int id)
        {
            return _repository.GetReadOnly(id);
        }
        public Region Add(Region region)
        {
            ArgumentNullException.ThrowIfNull(region);

            if (string.IsNullOrEmpty(region.Nome))
            {
                throw new ArgumentException("O nome da região é obrigatório", nameof(region.Nome));
            }

            try
            {
                _repository.Add(region);
                return region;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao adicionar região", e);
            }
        }

        public Region Update(int id, Region region)
        {
            ArgumentNullException.ThrowIfNull(region);

            var existingRegion = _repository.Get(id);

            if (existingRegion == null)
            {
                throw new KeyNotFoundException($"Região com id {id} não encontrada.");
            }

            try
            {
                existingRegion.Nome = region.Nome;

                _repository.Update(existingRegion);
                return existingRegion;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao alterar região", e);
            }
        }

        public void Delete(int id)
        {
            var existingRegion = _repository.Get(id);

            if (existingRegion == null)
            {
                throw new KeyNotFoundException($"Região com id {id} não encontrada.");
            }

            try
            {
                _repository.Delete(existingRegion);
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao excluir região", e);
            }
        }
    }
}

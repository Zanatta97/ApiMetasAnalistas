using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Services
{
    public class RegionService : IRegionService
    {
        private readonly IUnityOfWork _repository;
        public RegionService(IUnityOfWork repository)
        {
            _repository = repository;
        }
        public IEnumerable<Region> GetAll()
        {
            return _repository.RegionRepository.GetAll();
        }
        public Region? Get(int id)
        {
            return _repository.RegionRepository.Get(r => r.Id == id);
        }
        public Region? GetReadOnly(int id)
        {
            return _repository.RegionRepository.GetReadOnly(r => r.Id == id);
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
                _repository.RegionRepository.Add(region);
                _repository.Commit();
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

            var existingRegion = _repository.RegionRepository.Get(r => r.Id == id);

            if (existingRegion == null)
            {
                throw new KeyNotFoundException($"Região com id {id} não encontrada.");
            }

            try
            {
                existingRegion.Nome = region.Nome;

                _repository.RegionRepository.Update(existingRegion);
                _repository.Commit();
                return existingRegion;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao alterar região", e);
            }
        }

        public void Delete(int id)
        {
            var existingRegion = _repository.RegionRepository.Get(r => r.Id == id);

            if (existingRegion == null)
            {
                throw new KeyNotFoundException($"Região com id {id} não encontrada.");
            }

            try
            {
                _repository.RegionRepository.Delete(existingRegion);
                _repository.Commit();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao excluir região", e);
            }
        }
    }
}

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
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _repository.RegionRepository.GetAllAsync();
        }
        public async Task<Region?> GetAsync(int id)
        {
            return await _repository.RegionRepository.GetAsync(r => r.Id == id);
        }
        public async Task<Region?> GetReadOnlyAsync(int id)
        {
            return await _repository.RegionRepository.GetReadOnlyAsync(r => r.Id == id);
        }
        public async Task<Region> AddAsync(Region region)
        {
            ArgumentNullException.ThrowIfNull(region);

            if (string.IsNullOrEmpty(region.Nome))
            {
                throw new ArgumentException("O nome da região é obrigatório", nameof(region.Nome));
            }

            try
            {
                _repository.RegionRepository.Add(region);
                await _repository.Commit();
                return region;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao adicionar região", e);
            }
        }

        public async Task<Region> UpdateAsync(int id, Region region)
        {
            ArgumentNullException.ThrowIfNull(region);

            var existingRegion = await _repository.RegionRepository.GetAsync(r => r.Id == id);

            if (existingRegion == null)
            {
                throw new KeyNotFoundException($"Região com id {id} não encontrada.");
            }

            try
            {
                existingRegion.Nome = region.Nome;

                _repository.RegionRepository.Update(existingRegion);
                await _repository.Commit();
                return existingRegion;
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao alterar região", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingRegion = await _repository.RegionRepository.GetAsync(r => r.Id == id);

            if (existingRegion == null)
            {
                throw new KeyNotFoundException($"Região com id {id} não encontrada.");
            }

            try
            {
                _repository.RegionRepository.Delete(existingRegion);
                await _repository.Commit();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Erro ao excluir região", e);
            }
        }
    }
}

using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IRegionService
    {
        Task<IEnumerable<Region>> GetAllAsync();
        Task<Region?> GetAsync(int id);
        Task<Region?> GetReadOnlyAsync(int id);
        Task<Region> AddAsync(Region region);
        Task<Region> UpdateAsync(int id, Region region);
        Task DeleteAsync(int id);
    }
}

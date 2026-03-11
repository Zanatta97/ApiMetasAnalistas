using ApiMetasAnalistas.Models;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IRegionService
    {
        IEnumerable<Region> GetAll();
        Region? Get(int id);
        Region? GetReadOnly(int id);
        Region Add(Region region);
        Region Update(int id, Region region);
        void Delete(int id);
    }
}

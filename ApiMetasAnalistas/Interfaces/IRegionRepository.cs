using ApiMetasAnalistas.Models;
using System.Runtime.CompilerServices;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IRegionRepository
    {
        IEnumerable<Region> GetAll();
        Region? Get(int id);
        Region? GetReadOnly(int id);
        void Add(Region region);
        void Update(Region region);
        void Delete(Region region);
    }
}

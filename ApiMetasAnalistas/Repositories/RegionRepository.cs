using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly AppDBContext _context;

        public RegionRepository(AppDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Region> GetAll()
        {
            return _context.Regions.AsNoTracking().ToList();
        }

        public Region? Get(int id)
        {
            return _context.Regions.FirstOrDefault(r => r.Id == id);
        }

        public Region? GetReadOnly(int id)
        {
            return _context.Regions.AsNoTracking().FirstOrDefault(r => r.Id == id);
        }

        public void Add(Region region)
        {
            _context.Regions.Add(region);
            _context.SaveChanges();
        }

        public void Update(Region region)
        {
            _context.Regions.Update(region);
            _context.SaveChanges();
        }

        public void Delete(Region region)
        {
            _context.Regions.Remove(region);
            _context.SaveChanges();
        }
    }
}

using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using ApiMetasAnalistas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMetasAnalistas.Repositories
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        public RegionRepository(AppDBContext context) : base(context)
        {
        }
    }
}

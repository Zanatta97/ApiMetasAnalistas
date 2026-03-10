using ClientMetasAnalistas.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.Interfaces
{
    public interface IRegionService
    {
        public Task<IEnumerable<RegionDTO>> GetAllRegionsAsync();
        public Task<RegionDTO> GetRegionByIdAsync(int id);
    }
}

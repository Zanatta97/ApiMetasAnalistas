using ClientMetasAnalistas.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.Interfaces
{
    public interface IAnalystService
    {
        public Task<IEnumerable<AnalystResultDTO>> GetAllAnalystTargetDataAsync(DateTime startDate, DateTime endDate);
        public Task<bool> UsernameExistsAsync(string username);
        public Task<AnalystDTO> InsertAnalystAsync(AnalystDTO analystDTO);
        public Task<IEnumerable<AnalystDTO>> GetAllAnalysts();
        public Task<AnalystDTO> UpdateAnalyst(AnalystDTO analyst);
        public Task<AnalystResultDTO> GetAnalystTargetDataAsync(AnalystDTO analyst, DateTime startDate, DateTime endDate);
    }
}

using ClientMetasAnalistas.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.Interfaces
{
    internal interface IAnalystService
    {
        public Task<List<AnalystResultDTO>> GetAllAnalystTargetDataAsync(DateTime startDate, DateTime endDate);
    }
}

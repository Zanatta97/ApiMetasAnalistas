using ClientMetasAnalistas.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.Interfaces
{
    public interface IOccurrenceService
    {
        Task<IEnumerable<OccurrenceDTO>> GetOccurrences(int idAnalyst);
        Task<IEnumerable<OccurrenceDTO>> GetOccurrencesByAnalyst(int idAnalyst);
        Task<OccurrenceDTO> GetOccurrence(int id);
        Task<OccurrenceDTO> InsertOcurrenceAsync(OccurrenceDTO occurrenceDTO);
        Task<OccurrenceDTO> UpdateOcurrenceAsync(OccurrenceDTO occurrenceDTO);
        Task<bool> DeleteOccurrenceAsync(int id);
        Task<IEnumerable<OccurrenceDTO>> GetByAnalystAndPeriod(int idAnalyst, DateTime startDate, DateTime endDate);
        Task<bool> HasOccurrences(int id, DateTime occurrenceDate);
    }
}

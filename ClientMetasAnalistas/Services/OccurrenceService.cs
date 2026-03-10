using ClientMetasAnalistas.DTO;
using ClientMetasAnalistas.Interfaces;
using ClientMetasAnalistas.Utilidades;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace ClientMetasAnalistas.Services
{
    public class OccurrenceService: IOccurrenceService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<IEnumerable<OccurrenceDTO>> GetOccurrences(int idAnalyst)
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<OccurrenceDTO>>(Utils.API_URL + $"Occurrences/{idAnalyst}");
            return response ?? Enumerable.Empty<OccurrenceDTO>();
        }

        public async Task<IEnumerable<OccurrenceDTO>> GetOccurrencesByAnalyst(int idAnalyst)
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<OccurrenceDTO>>(Utils.API_URL + $"Occurrences/analyst/{idAnalyst}");
            return response ?? Enumerable.Empty<OccurrenceDTO>();
        }
    }
}

using ClientMetasAnalistas.DTO;
using ClientMetasAnalistas.Interfaces;
using ClientMetasAnalistas.Utilidades;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace ClientMetasAnalistas.Services
{
    public class OccurrenceService : IOccurrenceService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<IEnumerable<OccurrenceDTO>> GetOccurrences(int idAnalyst)
        {
            return await GetOccurrencesByAnalyst(idAnalyst);
        }

        public async Task<IEnumerable<OccurrenceDTO>> GetOccurrencesByAnalyst(int idAnalyst)
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<OccurrenceDTO>>(Utils.API_URL + $"Occurrences/analyst/{idAnalyst}");
            return response ?? Enumerable.Empty<OccurrenceDTO>();
        }

        public async Task<OccurrenceDTO> GetOccurrence(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<OccurrenceDTO>(Utils.API_URL + $"Occurrences/{id}");
            return response ?? new OccurrenceDTO();
        }

        public async Task<OccurrenceDTO> InsertOccurrenceAsync(OccurrenceDTO occurrenceDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(Utils.API_URL + "Occurrences", occurrenceDTO);

                if (response.IsSuccessStatusCode)
                {
                    var createdOccurrence = await response.Content.ReadFromJsonAsync<OccurrenceDTO>();
                    return createdOccurrence ?? new OccurrenceDTO { };
                }
                else
                {
                    Console.WriteLine($"Erro ao iserir ocorrência: {response.ReasonPhrase}");
                    return new OccurrenceDTO { };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao iserir ocorrência: {e.Message}");
                return new OccurrenceDTO { };
            }
        }

        public async Task<OccurrenceDTO> UpdateOccurrenceAsync(OccurrenceDTO occurrenceDTO)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(Utils.API_URL + $"Occurrences/{occurrenceDTO.Id}", occurrenceDTO);

                if (response.IsSuccessStatusCode)
                {
                    var updatedOccurrence = await response.Content.ReadFromJsonAsync<OccurrenceDTO>();
                    return updatedOccurrence ?? new OccurrenceDTO { };
                }
                else
                {
                    Console.WriteLine($"Erro ao atualizar ocorrência: {response.ReasonPhrase}");
                    return new OccurrenceDTO { };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao atualizar ocorrência: {e.Message}");
                return new OccurrenceDTO { };
            }
        }

        public async Task<bool> DeleteOccurrenceAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(Utils.API_URL + $"Occurrences/{id}");

                if (response.IsSuccessStatusCode)
                    return true;
                else
                {
                    Console.WriteLine($"Erro ao deletar ocorrência: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao deletar ocorrência: {e.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<OccurrenceDTO>> GetByPeriod(DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<OccurrenceDTO>>(Utils.API_URL + $"Occurrences/period?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? Enumerable.Empty<OccurrenceDTO>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao buscar ocorrências por período: {e.Message}");
                Utils.Break();
                return Enumerable.Empty<OccurrenceDTO>();
            }
        }

        public async Task<IEnumerable<OccurrenceDTO>> GetByAnalystAndPeriod(int idAnalyst, DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<OccurrenceDTO>>(Utils.API_URL + $"Occurrences/period/{idAnalyst}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
                return response ?? Enumerable.Empty<OccurrenceDTO>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao buscar ocorrências por analista e período: {e.Message}");
                Utils.Break();
                return Enumerable.Empty<OccurrenceDTO>();
            }
        }

        public async Task<bool> HasOccurrences(int id, DateTime occurrenceDate)
        {
            try
            {
                var response = await _httpClient.GetAsync(Utils.API_URL + $"Occurrences/hasOccurrence/{id}?occurrenceDate={occurrenceDate:yyyy-MM-dd}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<bool>();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao buscar ocorrências: {e.Message}");
                Utils.Break();
                return true;
            }
        }
    }
}

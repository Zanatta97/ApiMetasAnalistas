using ClientMetasAnalistas.DTO;
using ClientMetasAnalistas.Interfaces;
using ClientMetasAnalistas.UI;
using ClientMetasAnalistas.Utilidades;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Markup;

namespace ClientMetasAnalistas.Services
{
    public class AnalystService : IAnalystService
    {
        private readonly HttpClient _httpClient = new HttpClient();


        public async Task<IEnumerable<AnalystResultDTO>> GetAllAnalystTargetDataAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var query = $"?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

                var response = await _httpClient.GetFromJsonAsync<IEnumerable<AnalystResultDTO>>(Utils.API_URL + $"Analysts/target{query}");

                return response ?? Enumerable.Empty<AnalystResultDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter dados do analista: {ex.Message}");
                return Enumerable.Empty<AnalystResultDTO>();
            }
        }

        public async Task<AnalystResultDTO> GetAnalystTargetDataAsync(AnalystDTO analyst, DateTime startDate, DateTime endDate)
        {
            try
            {
                var query = $"?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

                var response = await _httpClient.GetFromJsonAsync<AnalystResultDTO>(Utils.API_URL + $"Analysts/target/{analyst.Id}{query}");

                return response ?? new AnalystResultDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter dados do analista: {ex.Message}");
                return new AnalystResultDTO();
            }
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            try
            {
                var response = await _httpClient.GetAsync(Utils.API_URL + $"Analysts/exists/{username}");

                if (response.IsSuccessStatusCode)
                {
                    var exists = response.Content.ReadAsStringAsync().Result;
                    return bool.Parse(exists);
                }
                else
                {
                    Console.WriteLine($"Erro ao verificar existência do username: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar existência do username: {ex.Message}");
                return false;
            }

        }

        public async Task<AnalystDTO> InsertAnalystAsync(AnalystDTO analystDTO)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync(Utils.API_URL + "Analysts", analystDTO);

                if (response.IsSuccessStatusCode)
                {
                    var createdAnalyst = await response.Content.ReadFromJsonAsync<AnalystDTO>();
                    return createdAnalyst ?? new AnalystDTO { };
                }
                else
                {
                    Console.WriteLine($"Erro ao inserir analista: {response.ReasonPhrase}");
                    return new AnalystDTO { };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir analista: {ex.Message}");
                return new AnalystDTO { };
            }
        }

        public async Task<IEnumerable<AnalystDTO>> GetAllAnalysts()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<AnalystDTO>>(Utils.API_URL + "Analysts");
                return response ?? Enumerable.Empty<AnalystDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter lista de analistas: {ex.Message}");
                return Enumerable.Empty<AnalystDTO>();
            }
        }

        public async Task<AnalystDTO> UpdateAnalyst(AnalystDTO analyst)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(Utils.API_URL + $"Analysts/{analyst.Id}", analyst);
                
                if (response.IsSuccessStatusCode)
                {
                    var updatedAnalyst = await response.Content.ReadFromJsonAsync<AnalystDTO>();
                    return updatedAnalyst ?? new AnalystDTO { };
                }
                else
                {
                    Console.WriteLine($"Erro ao atualizar analista: {response.ReasonPhrase}");
                    return new AnalystDTO { };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar analista: {ex.Message}");
                return new AnalystDTO { };
            }
        }
    }
}

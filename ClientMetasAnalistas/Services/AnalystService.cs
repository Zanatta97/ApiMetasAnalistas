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
    internal class AnalystService : IAnalystService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        //Método de IA para criar o meu depois
        public async Task<List<AnalystResultDTO>> GetAllAnalystTargetDataAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var query = $"?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

                var response = await _httpClient.GetFromJsonAsync<List<AnalystResultDTO>>(Utils.API_URL + $"Analysts/target{query}");
                
                return response ?? new List<AnalystResultDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter dados do analista: {ex.Message}");
                return new List<AnalystResultDTO>();
            }
        }
    }
}

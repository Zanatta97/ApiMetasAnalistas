using ClientMetasAnalistas.DTO;
using ClientMetasAnalistas.Interfaces;
using ClientMetasAnalistas.Utilidades;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace ClientMetasAnalistas.Services
{
    public class RegionService : IRegionService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<IEnumerable<RegionDTO>> GetAllRegionsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<RegionDTO>>(Utils.API_URL + "Regions");
                return response ?? Enumerable.Empty<RegionDTO>();
            }
            catch (Exception ex)
            {
                Utils.ImprimirAviso([$"Erro ao obter regiões! Veja Exceção a seguir"], "Cadastro Regiões", true);
                Utils.ImprimirTitulo("Cadastro Regiões");
                Console.WriteLine(ex.Message);
                Utils.Break();
                return Enumerable.Empty<RegionDTO>();
            }
        }

        public async Task<RegionDTO> GetRegionByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<RegionDTO>(Utils.API_URL + $"Regions/{id}");
                return response ?? new RegionDTO();
            }
            catch (Exception ex)
            {
                Utils.ImprimirAviso([$"Erro ao obter região por ID! Veja Exceção a seguir"], "Cadastro Regiões", true);
                Utils.ImprimirTitulo("Cadastro Regiões");
                Console.WriteLine(ex.Message);
                Utils.Break();
                return new RegionDTO();
            }
        }
    }
}

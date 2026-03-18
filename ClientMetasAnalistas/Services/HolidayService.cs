using ClientMetasAnalistas.DTO;
using ClientMetasAnalistas.Interfaces;
using ClientMetasAnalistas.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientMetasAnalistas.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<IEnumerable<HolidayDTO>> GetAllHolidaysAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<HolidayDTO>>(Utils.API_URL + "Holidays");
            return response ?? Enumerable.Empty<HolidayDTO>();
        }

        public async Task<HolidayDTO> GetHolidayAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<HolidayDTO>(Utils.API_URL + $"Holidays/{id}");
            return response ?? new HolidayDTO();
        }

        public async Task<HolidayDTO> InsertHolidayAsync(HolidayDTO holidayDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(Utils.API_URL + "Holidays", holidayDTO);

                if (response.IsSuccessStatusCode)
                {
                    var createdHoliday = await response.Content.ReadFromJsonAsync<HolidayDTO>();
                    return createdHoliday ?? new HolidayDTO { };
                }
                else
                {
                    Console.WriteLine($"Erro ao inserir feriado: {response.ReasonPhrase}");
                    return new HolidayDTO { };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao inserir feriado: {e.Message}");
                return new HolidayDTO { };
            }
        }

        public async Task<HolidayDTO> UpdateHolidayAsync(HolidayDTO holidayDTO)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(Utils.API_URL + $"Holidays/{holidayDTO.Id}", holidayDTO);

                if (response.IsSuccessStatusCode)
                {
                    var updatedHoliday = await response.Content.ReadFromJsonAsync<HolidayDTO>();
                    return updatedHoliday ?? new HolidayDTO { };
                }
                else
                {
                    Console.WriteLine($"Erro ao atualizar feriado: {response.ReasonPhrase}");
                    return new HolidayDTO { };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao atualizar feriado: {e.Message}");
                return new HolidayDTO { };
            }
        }

        public async Task<bool> DeleteHolidayAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(Utils.API_URL + $"Holidays/{id}");

                if (response.IsSuccessStatusCode)
                    return true;
                else
                {
                    Console.WriteLine($"Erro ao deletar feriado: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao deletar feriado: {e.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<HolidayDTO>> GetHolidaysByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<HolidayDTO>>(Utils.API_URL + $"Holidays/period?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            return response ?? Enumerable.Empty<HolidayDTO>();
        }
    }
}
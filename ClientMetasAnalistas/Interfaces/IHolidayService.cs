using ClientMetasAnalistas.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientMetasAnalistas.Interfaces
{
    public interface IHolidayService
    {
        Task<IEnumerable<HolidayDTO>> GetAllHolidaysAsync();
        Task<HolidayDTO> GetHolidayAsync(int id);
        Task<HolidayDTO> InsertHolidayAsync(HolidayDTO holidayDTO);
        Task<HolidayDTO> UpdateHolidayAsync(HolidayDTO holidayDTO);
        Task<bool> DeleteHolidayAsync(int id);
        Task<IEnumerable<HolidayDTO>> GetHolidaysByPeriodAsync(DateTime startDate, DateTime endDate);
    }
}
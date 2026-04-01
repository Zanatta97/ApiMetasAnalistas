using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.DTO
{
    public class HolidayRequestDTO
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int RegiaoId { get; set; }
    }
}

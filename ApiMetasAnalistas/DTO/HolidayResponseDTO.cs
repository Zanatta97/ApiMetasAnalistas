using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.DTO
{
    public class HolidayResponseDTO
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int RegiaoId { get; set; }
    }
}

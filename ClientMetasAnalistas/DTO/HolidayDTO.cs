using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.DTO
{
    public class HolidayDTO
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string? Descricao { get; set; }
        public int RegiaoId { get; set; }
    }
}

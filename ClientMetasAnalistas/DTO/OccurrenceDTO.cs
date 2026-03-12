using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.DTO
{
    public class OccurrenceDTO
    {
        public int Id { get; set; }
        public int Tipo { get; set; }
        public string? Descricao { get; set; }
        public int AnalistaId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}

using ApiMetasAnalistas.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.DTO
{
    public class OccurrenceRequestDTO
    {
        public int Tipo { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int AnalistaId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

    }
}

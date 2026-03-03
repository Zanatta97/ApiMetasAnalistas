using System.ComponentModel.DataAnnotations;

namespace ApiMetasAnalistas.Models
{
    public class Occurrence
    {
        public int Id { get; set; }
        [Required]
        public int Tipo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public int AnalistaId { get; set; }
        public Analyst? Analista { get; set; }
        [Required]
        public DateTime DataInicio { get; set; }
        [Required]
        public DateTime DataFim { get; set; }

        public Occurrence(int tipo, string descricao, int analistaId, DateTime dataInicio, DateTime dataFim)
        {
            Tipo = tipo;
            Descricao = descricao;
            AnalistaId = analistaId;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }
    }
}

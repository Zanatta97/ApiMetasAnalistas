using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.Models
{
    [Table("occurrences")]
    public class Occurrence
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(1)]
        [Column("tipo")]
        public int Tipo { get; set; }

        [Required]
        [StringLength(200)]
        [Column("descricao")]
        public string Descricao { get; set; }

        [Required]
        [Column("analista_id")]
        public int AnalistaId { get; set; }

        public Analyst? Analista { get; set; }

        [Required]
        [Column("data_inicio")]
        public DateTime DataInicio { get; set; }

        [Required]
        [Column("data_fim")]
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

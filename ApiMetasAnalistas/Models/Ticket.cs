using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.Models
{
    [Table("tickets")]
    public class Ticket
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("analista_id")]
        public int AnalystId { get; set; }

        public Analyst? Analyst { get; set; }

        [Required]
        [Column("data_fechamento")]
        public DateTime DataFechamento { get; set; }

        public Ticket(int analystId, DateTime dataFechamento)
        {
                AnalystId = analystId;
                DataFechamento = dataFechamento;
        }
    }
}

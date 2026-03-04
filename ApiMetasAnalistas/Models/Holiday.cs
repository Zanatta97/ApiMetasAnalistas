using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.Models
{
    [Table("holidays")]
    public class Holiday
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("data")]
        public DateTime Data { get; set; }

        [Required]
        [StringLength(200)]
        [Column("descricao")]
        public string Descricao { get; set; }

        [Required]
        [Column("regiao_id")]
        public int RegiaoId { get; set; }

        public Region? Regiao { get; set; }

        public Holiday(DateTime data, string descricao, int regiaoId)
        {
            Data = data;
            Descricao = descricao;
            RegiaoId = regiaoId;
        }
    }
}

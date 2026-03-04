using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.Models
{
    [Table("analysts")]
    [Index(nameof(Usuario), IsUnique = true)]
    public class Analyst
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        [Column("nome")]
        public string Nome { get; set; }

        [Required]
        [StringLength(60)]
        [Column("usuario")]
        public string Usuario { get; set; }

        [Required]
        [Column("regiao_id")]
        public int RegiaoId { get; set; }

        public Region? Regiao { get; set; }
        
        [Required]
        [Range(0, 99, ErrorMessage = "O campo deve ter no máximo 2 dígitos.")]
        [Column("meta_diaria", TypeName = "numeric(2, 0)")]
        public int MetaDiaria { get; set; }

        public Analyst(string nome, string usuario, int regiaoId, int metaDiaria)
        {
            Nome = nome;
            Usuario = usuario;
            RegiaoId = regiaoId;
            MetaDiaria = metaDiaria;
        }

    }
}

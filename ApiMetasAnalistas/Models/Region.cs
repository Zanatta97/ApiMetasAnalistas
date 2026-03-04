using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.Models
{
    [Table("regions")]
    public class Region
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("nome")]
        public string Nome { get; set; }

        public Region(string nome)
        {
            Nome = nome;
        }
    }

}

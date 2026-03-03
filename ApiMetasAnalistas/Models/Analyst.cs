using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.Models
{
    public class Analyst
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string RegiaoId { get; set; }
        public Region? Regiao { get; set; }
        public int MetaDiaria { get; set; }

        public Analyst(string nome, string usuario, string regiaoId, int metaDiaria)
        {
            Nome = nome;
            Usuario = usuario;
            RegiaoId = regiaoId;
            MetaDiaria = metaDiaria;
        }

    }
}

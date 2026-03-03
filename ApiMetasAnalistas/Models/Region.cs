using System.ComponentModel.DataAnnotations;

namespace ApiMetasAnalistas.Models
{
    public class Region
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }

        public Region(string nome)
        {
            Nome = nome;
        }
    }

}

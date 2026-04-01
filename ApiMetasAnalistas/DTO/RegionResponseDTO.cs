using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.DTO
{
    public class RegionResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}

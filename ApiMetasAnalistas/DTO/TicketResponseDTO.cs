using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.DTO
{
    public class TicketResponseDTO
    {
        public int Id { get; set; }
        public int AnalystId { get; set; }
        public DateTime DataFechamento { get; set; }
    }
}

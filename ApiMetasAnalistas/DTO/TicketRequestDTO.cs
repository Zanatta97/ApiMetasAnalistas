using ApiMetasAnalistas.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMetasAnalistas.DTO
{
    public class TicketRequestDTO
    {
        public int AnalystId { get; set; }
        public DateTime DataFechamento { get; set; }
    }
}

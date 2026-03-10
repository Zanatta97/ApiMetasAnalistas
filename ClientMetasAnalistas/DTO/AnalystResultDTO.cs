using System;
using System.Collections.Generic;
using System.Text;

namespace ClientMetasAnalistas.DTO
{
    public class AnalystResultDTO
    {
        public int AnalistaId { get; set; }
        public string NomeAnalista { get; set; }
        public int RegiaoId { get; set; }
        public int TotalDiasUteis { get; set; }
        public int MetaDiaria { get; set; }
        public int TotalMetaPeriodo { get; set; }
        public int TicketsFechados { get; set; }
        public decimal PercentualMetaAlcancada { get; set; }
    }
}

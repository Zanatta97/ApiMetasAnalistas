using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClientMetasAnalistas.DTO
{
    public class AnalystDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public int RegiaoId { get; set; }
        public int MetaDiaria { get; set; }
    }
}

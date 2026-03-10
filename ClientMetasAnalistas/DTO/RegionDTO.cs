using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClientMetasAnalistas.DTO
{
    public class RegionDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}

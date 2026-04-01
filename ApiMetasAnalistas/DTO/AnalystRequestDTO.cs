namespace ApiMetasAnalistas.DTO
{
    public class AnalystRequestDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public int RegiaoId { get; set; }
        public int MetaDiaria { get; set; }
    }
}

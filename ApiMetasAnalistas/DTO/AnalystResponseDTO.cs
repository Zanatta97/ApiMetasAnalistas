namespace ApiMetasAnalistas.DTO
{
    public class AnalystResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public int RegiaoId { get; set; }
        public string? NomeRegiao { get; set; }
        public int MetaDiaria { get; set; }
    }
}

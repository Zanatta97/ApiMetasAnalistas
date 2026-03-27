namespace ApiMetasAnalistas.DTO
{
    public class OccurrenceResponseDTO
    {
        public int Id { get; set; }
        public int Tipo { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int AnalistaId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}

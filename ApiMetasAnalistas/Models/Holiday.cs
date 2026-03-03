namespace ApiMetasAnalistas.Models
{
    public class Holiday
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }

        public int RegiaoId { get; set; }
        public Region? Regiao { get; set; }

        public Holiday(DateTime data, string descricao, int regiaoId)
        {
            Data = data;
            Descricao = descricao;
            RegiaoId = regiaoId;
        }
    }
}

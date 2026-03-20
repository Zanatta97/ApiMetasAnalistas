using System.Text.Json;

namespace ApiMetasAnalistas.DTO
{
    public class ErrorResponseDTO
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string? StatusMessage { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

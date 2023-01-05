using System.Text.Json;

namespace Domain.ErrorModel
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? ExceptionMessage { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}

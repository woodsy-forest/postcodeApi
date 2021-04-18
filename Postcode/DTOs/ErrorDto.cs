using System.Text.Json.Serialization;

namespace Postcode.Dto
{
    public class ErrorDto
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

    }
}

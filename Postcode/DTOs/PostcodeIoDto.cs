using System.Text.Json.Serialization;

namespace Postcode.Dto
{
    public class PostcodeIoDto
    {
        public PostcodeIoDto()
        {
            Result = new ResultDto();
        }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("result")]
        public ResultDto Result { get; set; }

    }

    public class ResultDto
    {
        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
    }
}

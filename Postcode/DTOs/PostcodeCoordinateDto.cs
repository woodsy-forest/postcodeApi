using System.Text.Json.Serialization;

namespace Postcode.Dto
{
    public class PostcodeCoordinateDto
    {
        public PostcodeCoordinateDto()
        {
            Coordinates = new CoordinateDto();
        }

        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }

        [JsonPropertyName("coordinates")]
        public CoordinateDto Coordinates { get; set; }

    }

    public class CoordinateDto
    {
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
    }
}

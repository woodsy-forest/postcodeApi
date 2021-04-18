using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Postcode.Dto
{
    public class PostcodeBulkIoDto
    {
        public PostcodeBulkIoDto()
        {
            Result = new List<BulkResultDto>();
        }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("result")]
        public List<BulkResultDto> Result { get; set; }

    }

    public class BulkResultDto
    {
        public BulkResultDto()
        {
            Result = new QueryBulkResultDto();
        }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("result")]
        public QueryBulkResultDto Result { get; set; }
    }

    public class QueryBulkResultDto
    {
        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
    }
}

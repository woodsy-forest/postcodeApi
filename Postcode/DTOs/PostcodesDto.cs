using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Postcode.Dto
{
    public class PostcodesDto
    {

        public PostcodesDto()
        {
            Postcodes = new List<string>();
        }

        [JsonPropertyName("postcodes")]
        public List<string> Postcodes { get; set; }

    }
}

using System.Text.Json.Serialization;

namespace TrafficLightAPI.Models
{
    public class ObservationResponse
    {
        [JsonPropertyName("start")]
        public List<int>? Start { get; set; }
        [JsonPropertyName("missing")]
        public string[]? Missing { get; set; }
    }
}

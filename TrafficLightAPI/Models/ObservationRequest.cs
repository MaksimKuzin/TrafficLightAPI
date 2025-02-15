using System.Text.Json.Serialization;

namespace TrafficLightAPI.Models
{
    public class ObservationRequest
    {
        [JsonPropertyName("observation")]
        public Observation Observation { get; set; } = new Observation();

        [JsonPropertyName("sequence")]
        public Guid SequenceId { get; set; } = Guid.Empty;
    }

    public class Observation
    {
        [JsonPropertyName("color")]
        public string Color { get; set; } = string.Empty;
        [JsonPropertyName("numbers")]
        public string[]? Segments { get; set; }
    }
}

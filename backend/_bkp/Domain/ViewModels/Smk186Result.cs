using System.Text.Json.Serialization;

namespace CblxChallenge.Domain.ViewModels
{
    public class Smk186Result
    {
        [JsonPropertyName("a")]
        public double AMineralInTon { get; set; }

        [JsonPropertyName("b")]
        public double BMineralInTon { get; set; }

        [JsonPropertyName("c")]
        public double CMineralInTon { get; set; }

        [JsonPropertyName("d")]
        public double DMineralInTon { get; set; }
    }
}

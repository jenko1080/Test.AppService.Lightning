using System.Text.Json.Serialization;

namespace Test.AppService.Lightning.API.Models
{
    public class KeepAliveMessage
    {
        [JsonPropertyName("ts")]
        public DateTime DateTimeUtc { get; set; }

        [JsonPropertyName("st")]
        public LightningStrokeType Type { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Test.AppService.Lightning.API.Models
{
    public class LightningStrokeEntry : TableEntityBase
    {
        // Strike timestamp (format: 2022-03-15 04:45:36.490)
        [JsonPropertyName("ts")]
        public DateTime DateTimeUtc { get; set; }

        // Latitude
        [JsonPropertyName("lat")]
        public float Latitude { get; set; }
        
        // Longitude
        [JsonPropertyName("lon")]
        public float Longitude { get; set; }

        // Amplitude
        [JsonPropertyName("kamp")]
        public float Amplitude { get; set; }
        
        // Strike Type
        [JsonPropertyName("st")]
        public LightningStrokeType Type { get; set; }

        // Strike Height (relevant for `in cloud`)
        [JsonPropertyName("km")]
        public float Height { get; set; }

        // Number of sensors
        [JsonPropertyName("sen")]
        public int NumSensors { get; set; }

        // Number of pulses
        [JsonPropertyName("pul")]
        public int NumPulses { get; set; }

        public override void SetRowAndPartitionKeys()
        {
            PartitionKey = DateTimeUtc.ToString("yyyy-MM-dd");
            RowKey = DateTimeUtc.ToString("HH:mm:ss.fff") + $"_{Latitude}_{Longitude}";
        }
    }
}

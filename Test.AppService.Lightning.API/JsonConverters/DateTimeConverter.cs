using System.Text.Json;
using System.Text.Json.Serialization;

namespace Test.AppService.Lightning.API.JsonConverters
{
    /// <summary>
    /// Converter for formats like: `2022-03-15 04:45:36.490` (assumes UTC)
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Parse datetime
            DateTime.TryParse(reader.GetString(), out var dt);

            // Return datetime as UTC
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}

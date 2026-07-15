using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13;

public class JsonDateTimeConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd";
    
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string dateStr = reader.GetString();
        if (DateTime.TryParseExact(dateStr, Format, null, System.Globalization.DateTimeStyles.None, out DateTime result))
        {
            return result;
        }
        throw new JsonException($"Неверный формат даты. Ожидался формат {Format}");
    }
    
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}
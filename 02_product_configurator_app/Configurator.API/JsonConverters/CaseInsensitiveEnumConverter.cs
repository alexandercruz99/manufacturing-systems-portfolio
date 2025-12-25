using System.Text.Json;
using System.Text.Json.Serialization;

namespace Configurator.API.JsonConverters;

public class CaseInsensitiveEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Console.WriteLine($"[CONVERTER] Read called for {typeof(T).Name}, TokenType: {reader.TokenType}");
        
        if (reader.TokenType != JsonTokenType.String)
        {
            Console.WriteLine($"[CONVERTER] ERROR: Expected String token, got {reader.TokenType}");
            throw new JsonException($"Expected string token, got {reader.TokenType}");
        }

        var value = reader.GetString();
        Console.WriteLine($"[CONVERTER] Received string value: '{value}' for {typeof(T).Name}");
        
        if (string.IsNullOrEmpty(value))
        {
            Console.WriteLine($"[CONVERTER] ERROR: Null or empty string");
            throw new JsonException($"Cannot convert null or empty string to {typeof(T).Name}");
        }

        // Try case-insensitive enum parsing
        if (Enum.TryParse<T>(value, ignoreCase: true, out var result))
        {
            Console.WriteLine($"[CONVERTER] SUCCESS: Parsed '{value}' to {result} for {typeof(T).Name}");
            return result;
        }

        var validValues = string.Join(", ", Enum.GetNames(typeof(T)));
        Console.WriteLine($"[CONVERTER] ERROR: Cannot parse '{value}'. Valid values: {validValues}");
        throw new JsonException($"Cannot convert '{value}' to {typeof(T).Name}. Valid values are: {validValues}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}


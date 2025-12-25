using System.Text.Json;
using System.Text.Json.Serialization;
using Configurator.Core.Enums;

namespace Configurator.API.JsonConverters;

public class ConfigOptionConverter : JsonConverter<ConfigOption>
{
    public override ConfigOption Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            throw new JsonException($"Cannot convert null or empty string to ConfigOption");
        }

        // Try case-insensitive enum parsing
        if (Enum.TryParse<ConfigOption>(value, ignoreCase: true, out var result))
        {
            return result;
        }

        throw new JsonException($"Cannot convert '{value}' to ConfigOption. Valid values are: {string.Join(", ", Enum.GetNames(typeof(ConfigOption)))}");
    }

    public override void Write(Utf8JsonWriter writer, ConfigOption value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}







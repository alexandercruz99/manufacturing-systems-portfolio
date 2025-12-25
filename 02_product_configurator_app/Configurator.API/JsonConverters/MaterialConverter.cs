using System.Text.Json;
using System.Text.Json.Serialization;
using Configurator.Core.Enums;

namespace Configurator.API.JsonConverters;

public class MaterialConverter : JsonConverter<Material>
{
    public override Material Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            throw new JsonException($"Cannot convert null or empty string to Material");
        }

        // Try case-insensitive enum parsing
        if (Enum.TryParse<Material>(value, ignoreCase: true, out var result))
        {
            return result;
        }

        throw new JsonException($"Cannot convert '{value}' to Material. Valid values are: {string.Join(", ", Enum.GetNames(typeof(Material)))}");
    }

    public override void Write(Utf8JsonWriter writer, Material value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}







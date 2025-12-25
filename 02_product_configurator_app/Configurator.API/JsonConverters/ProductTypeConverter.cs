using System.Text.Json;
using System.Text.Json.Serialization;
using Configurator.Core.Enums;

namespace Configurator.API.JsonConverters;

public class ProductTypeConverter : JsonConverter<ProductType>
{
    public override ProductType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected string token, got {reader.TokenType}");
        }

        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            throw new JsonException($"Cannot convert null or empty string to ProductType");
        }

        // Try case-insensitive enum parsing
        if (Enum.TryParse<ProductType>(value, ignoreCase: true, out var result))
        {
            return result;
        }

        throw new JsonException($"Cannot convert '{value}' to ProductType. Valid values are: {string.Join(", ", Enum.GetNames(typeof(ProductType)))}");
    }

    public override void Write(Utf8JsonWriter writer, ProductType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}


using System.Text.Json;
using System.Text.Json.Serialization;

namespace Configurator.API.JsonConverters;

public class CaseInsensitiveEnumConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        var result = typeToConvert.IsEnum;
        Console.WriteLine($"[FACTORY] CanConvert called for type: {typeToConvert.Name}, IsEnum: {typeToConvert.IsEnum}, Result: {result}");
        return result;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Console.WriteLine($"[FACTORY] CreateConverter called for type: {typeToConvert.Name}");
        var converterType = typeof(CaseInsensitiveEnumConverter<>).MakeGenericType(typeToConvert);
        var converter = (JsonConverter)Activator.CreateInstance(converterType)!;
        Console.WriteLine($"[FACTORY] Created converter: {converter.GetType().Name}");
        return converter;
    }
}


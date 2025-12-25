using Configurator.API.Controllers;
using Configurator.Core.Enums;
using Configurator.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure logging first so we can use it
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var logger = LoggerFactory.Create(config => config.AddConsole().AddDebug()).CreateLogger("Program");

builder.Services.AddControllers(options =>
    {
        // Suppress model state validation filter to allow manual deserialization
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        // CRITICAL: Use namingPolicy: null to accept EXACT enum names (PascalCase like "FanCoil")
        // The default JsonStringEnumConverter() uses camelCase which expects "fanCoil"
        options.JsonSerializerOptions.Converters.Clear();
        options.JsonSerializerOptions.Converters.Insert(0, new JsonStringEnumConverter(namingPolicy: null, allowIntegerValues: false));
        Console.WriteLine($"[PROGRAM] Configured JsonSerializerOptions with {options.JsonSerializerOptions.Converters.Count} converters");
        Console.WriteLine($"[PROGRAM] JsonStringEnumConverter configured with namingPolicy=null (exact enum names)");
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // Disable automatic model state validation - we'll handle it manually
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product Configurator API",
        Version = "v1",
        Description = "Manufacturing product configurator for coils, fan coils, and unit heaters"
    });
    
    // Ensure request body schemas are properly generated
    c.CustomSchemaIds(type => type.FullName);
    
    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
    if (System.IO.File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    
    // Configure Swagger to use string enums (matches our JsonStringEnumConverter)
    c.MapType<ProductType>(() => new Microsoft.OpenApi.Models.OpenApiSchema 
    { 
        Type = "string", 
        Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny> 
        { 
            new Microsoft.OpenApi.Any.OpenApiString("Coil"),
            new Microsoft.OpenApi.Any.OpenApiString("FanCoil"),
            new Microsoft.OpenApi.Any.OpenApiString("UnitHeater")
        }
    });
    c.MapType<Material>(() => new Microsoft.OpenApi.Models.OpenApiSchema 
    { 
        Type = "string", 
        Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny> 
        { 
            new Microsoft.OpenApi.Any.OpenApiString("Aluminum"),
            new Microsoft.OpenApi.Any.OpenApiString("Copper"),
            new Microsoft.OpenApi.Any.OpenApiString("Steel")
        }
    });
    c.MapType<ConfigOption>(() => new Microsoft.OpenApi.Models.OpenApiSchema 
    { 
        Type = "string", 
        Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny> 
        { 
            new Microsoft.OpenApi.Any.OpenApiString("None"),
            new Microsoft.OpenApi.Any.OpenApiString("Coating"),
            new Microsoft.OpenApi.Any.OpenApiString("StainlessFasteners"),
            new Microsoft.OpenApi.Any.OpenApiString("HighEfficiencyFins"),
            new Microsoft.OpenApi.Any.OpenApiString("ExpressBuild")
        }
    });
});

var app = builder.Build();

// Test enum conversion directly
Console.Out.Flush();
System.Diagnostics.Debug.WriteLine("=== TEST START ===");
Console.WriteLine("=== TEST START ===");
try
{
    var testOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(namingPolicy: null, allowIntegerValues: false) }
    };
    
    var testJson = """{"productType":"FanCoil","widthIn":24.0,"heightIn":18.0,"depthIn":12.0,"material":"Copper","options":["Coating"],"quantity":10}""";
    Console.WriteLine($"[TEST] JSON: {testJson}");
    
    var result = JsonSerializer.Deserialize<ConfiguratorRequest>(testJson, testOptions);
    if (result != null)
    {
        Console.WriteLine($"[TEST] SUCCESS! ProductType={result.ProductType}, Material={result.Material}");
    }
    else
    {
        Console.WriteLine("[TEST] FAILED: null result");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"[TEST] EXCEPTION: {ex.GetType().Name} - {ex.Message}");
}
Console.WriteLine("=== TEST END ===");
System.Diagnostics.Debug.WriteLine("=== TEST END ===");
Console.Out.Flush();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Configurator API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();


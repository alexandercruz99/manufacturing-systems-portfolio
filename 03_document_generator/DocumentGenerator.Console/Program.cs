using Configurator.Core.Enums;
using Configurator.Core.Models;
using Configurator.Core.Pricing;
using DocGen = DocumentGenerator.Core.DocumentGenerator;

var request = new ConfiguratorRequest
{
    ProductType = ProductType.FanCoil,
    WidthIn = 24.0m,
    HeightIn = 18.0m,
    DepthIn = 12.0m,
    Material = Material.Copper,
    Options = new List<ConfigOption> { ConfigOption.Coating, ConfigOption.StainlessFasteners },
    Quantity = 10
};

var result = PricingEngine.Price(request);

var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
Directory.CreateDirectory(outputDir);

var salesSheetPath = Path.Combine(outputDir, "SalesSheet.pdf");
var plantInstructionsPath = Path.Combine(outputDir, "PlantInstructions.pdf");

DocGen.GenerateSalesSheet(result, salesSheetPath);
DocGen.GeneratePlantInstructions(result, plantInstructionsPath);

Console.WriteLine($"Configuration ID: {result.ConfigurationId}");
Console.WriteLine($"Sales Sheet generated: {salesSheetPath}");
Console.WriteLine($"Plant Instructions generated: {plantInstructionsPath}");


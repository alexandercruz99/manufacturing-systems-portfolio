using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Configurator.Core.Enums;
using Configurator.Core.Models;

namespace Configurator.Core.Pricing;

public static class PricingEngine
{
    private const decimal BasePricePerCubicInch = 0.15m;
    private const decimal MinimumPrice = 250.00m;

    private static readonly Dictionary<ProductType, decimal> ProductMultipliers = new()
    {
        { ProductType.Coil, 1.0m },
        { ProductType.FanCoil, 1.35m },
        { ProductType.UnitHeater, 1.65m }
    };

    private static readonly Dictionary<Material, decimal> MaterialMultipliers = new()
    {
        { Material.Aluminum, 1.0m },
        { Material.Copper, 1.85m },
        { Material.Steel, 1.25m }
    };

    private static readonly Dictionary<ConfigOption, decimal> OptionAdders = new()
    {
        { ConfigOption.None, 0.0m },
        { ConfigOption.Coating, 45.00m },
        { ConfigOption.StainlessFasteners, 28.00m },
        { ConfigOption.HighEfficiencyFins, 125.00m },
        { ConfigOption.ExpressBuild, 200.00m }
    };

    private static readonly Dictionary<int, decimal> QuantityDiscounts = new()
    {
        { 1, 0.0m },
        { 5, 0.05m },
        { 10, 0.10m },
        { 25, 0.15m },
        { 50, 0.20m }
    };

    public static ConfiguratorResult Price(ConfiguratorRequest request)
    {
        var volume = request.WidthIn * request.HeightIn * request.DepthIn;
        var basePrice = volume * BasePricePerCubicInch;

        var productMultiplier = ProductMultipliers[request.ProductType];
        var materialMultiplier = MaterialMultipliers[request.Material];

        var unitPrice = basePrice * productMultiplier * materialMultiplier;

        var optionAdder = request.Options
            .Where(o => o != ConfigOption.None)
            .Sum(o => OptionAdders[o]);

        unitPrice += optionAdder;

        if (unitPrice < MinimumPrice)
        {
            unitPrice = MinimumPrice;
        }

        var discount = GetQuantityDiscount(request.Quantity);
        unitPrice *= (1.0m - discount);

        var extendedPrice = unitPrice * request.Quantity;

        var bom = GenerateBom(request);

        var configurationId = GenerateConfigurationId(request);

        return new ConfiguratorResult
        {
            ConfigurationId = configurationId,
            ProductType = request.ProductType,
            Material = request.Material,
            UnitPrice = Math.Round(unitPrice, 2),
            ExtendedPrice = Math.Round(extendedPrice, 2),
            Bom = bom,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    private static decimal GetQuantityDiscount(int quantity)
    {
        return QuantityDiscounts
            .Where(kvp => quantity >= kvp.Key)
            .OrderByDescending(kvp => kvp.Key)
            .FirstOrDefault().Value;
    }

    private static List<LineItem> GenerateBom(ConfiguratorRequest request)
    {
        var bom = new List<LineItem>
        {
            new LineItem { Code = "FRAME-001", Description = $"{request.ProductType} Frame Assembly", Qty = request.Quantity },
            new LineItem { Code = "MAT-001", Description = $"{request.Material} Core Material", Qty = request.Quantity }
        };

        if (request.Options.Contains(ConfigOption.Coating))
        {
            bom.Add(new LineItem { Code = "OPT-COAT", Description = "Protective Coating", Qty = request.Quantity });
        }

        if (request.Options.Contains(ConfigOption.StainlessFasteners))
        {
            bom.Add(new LineItem { Code = "OPT-SS", Description = "Stainless Steel Fasteners", Qty = request.Quantity });
        }

        if (request.Options.Contains(ConfigOption.HighEfficiencyFins))
        {
            bom.Add(new LineItem { Code = "OPT-HEF", Description = "High Efficiency Fins", Qty = request.Quantity });
        }

        if (request.Options.Contains(ConfigOption.ExpressBuild))
        {
            bom.Add(new LineItem { Code = "OPT-EXP", Description = "Express Build Flag", Qty = 1 });
        }

        return bom;
    }

    private static string GenerateConfigurationId(ConfiguratorRequest request)
    {
        var normalized = new
        {
            ProductType = request.ProductType.ToString(),
            WidthIn = request.WidthIn.ToString("F2"),
            HeightIn = request.HeightIn.ToString("F2"),
            DepthIn = request.DepthIn.ToString("F2"),
            Material = request.Material.ToString(),
            Options = request.Options.OrderBy(o => o.ToString()).Select(o => o.ToString()).ToList(),
            Quantity = request.Quantity
        };

        var json = JsonSerializer.Serialize(normalized);
        var bytes = Encoding.UTF8.GetBytes(json);
        var hash = SHA256.HashData(bytes);
        var hashString = Convert.ToHexString(hash).ToLowerInvariant();
        return $"CFG-{hashString[..12]}";
    }
}


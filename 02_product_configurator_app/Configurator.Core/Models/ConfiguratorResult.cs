using Configurator.Core.Enums;

namespace Configurator.Core.Models;

public record ConfiguratorResult
{
    public string ConfigurationId { get; init; } = string.Empty;
    public ProductType ProductType { get; init; }
    public Material Material { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal ExtendedPrice { get; init; }
    public List<LineItem> Bom { get; init; } = new();
    public DateTime CreatedAtUtc { get; init; }
}


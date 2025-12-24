using Configurator.Core.Enums;

namespace Configurator.Core.Models;

public record ConfiguratorRequest
{
    public ProductType ProductType { get; init; }
    public decimal WidthIn { get; init; }
    public decimal HeightIn { get; init; }
    public decimal DepthIn { get; init; }
    public Material Material { get; init; }
    public List<ConfigOption> Options { get; init; } = new();
    public int Quantity { get; init; }
}


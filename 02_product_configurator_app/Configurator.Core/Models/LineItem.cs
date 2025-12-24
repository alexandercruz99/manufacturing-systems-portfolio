namespace Configurator.Core.Models;

public record LineItem
{
    public string Code { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int Qty { get; init; }
}


using System.ComponentModel.DataAnnotations;

namespace MockErp.API.Models;

public class ErpOrderRequest
{
    [Required]
    public string OrderId { get; set; } = string.Empty;

    [Required]
    public string ConfigurationId { get; set; } = string.Empty;

    [Required]
    public List<ErpOrderItem> Items { get; set; } = new();

    [Required]
    public DateTime RequestedShipDate { get; set; }

    public List<string> RoutingFlags { get; set; } = new();

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Total price must be greater than or equal to 0.")]
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// The expected extended price from the Configurator API result.
    /// This is used to validate that the totalPrice matches the configuration pricing.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Expected extended price must be greater than or equal to 0.")]
    public decimal? ExpectedExtendedPrice { get; set; }
}

public class ErpOrderItem
{
    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }
}


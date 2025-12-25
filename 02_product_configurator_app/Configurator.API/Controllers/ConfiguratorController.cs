using Configurator.Core.Models;
using Configurator.Core.Pricing;
using Configurator.Core.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Configurator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = false)]
public class ConfiguratorController : ControllerBase
{
    private readonly ILogger<ConfiguratorController> _logger;

    public ConfiguratorController(ILogger<ConfiguratorController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Calculates the price for a product configuration
    /// </summary>
    /// <param name="request">The configuration request containing product type, dimensions, material, options, and quantity</param>
    /// <returns>A configuration result with pricing information</returns>
    /// <response code="200">Configuration priced successfully</response>
    /// <response code="400">Invalid request data or validation failed</response>
    /// <response code="500">An error occurred while processing the request</response>
    [HttpPost("price")]
    [ProducesResponseType(typeof(ConfiguratorResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public IActionResult Price([FromBody] ConfiguratorRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { error = "Request body is required." });
        }

        var (isValid, errors) = Validator.Validate(request);
        if (!isValid)
        {
            return BadRequest(new { error = "Validation failed.", details = errors });
        }

        try
        {
            var result = PricingEngine.Price(request);
            _logger.LogInformation(
                "Configuration priced successfully. ConfigurationId: {ConfigurationId}, ProductType: {ProductType}, UnitPrice: {UnitPrice}, ExtendedPrice: {ExtendedPrice}",
                result.ConfigurationId,
                result.ProductType,
                result.UnitPrice,
                result.ExtendedPrice);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error pricing configuration.");
            return StatusCode(500, new { error = "An error occurred while pricing the configuration." });
        }
    }

    /// <summary>
    /// Validates a product configuration request
    /// </summary>
    /// <param name="request">The configuration request to validate</param>
    /// <returns>A validation result indicating if the request is valid</returns>
    /// <response code="200">Request is valid</response>
    /// <response code="400">Validation failed</response>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public IActionResult Validate([FromBody] ConfiguratorRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { error = "Request body is required." });
        }

        var (isValid, errors) = Validator.Validate(request);
        if (!isValid)
        {
            return BadRequest(new { error = "Validation failed.", details = errors });
        }

        return Ok(new { message = "Request is valid." });
    }
}

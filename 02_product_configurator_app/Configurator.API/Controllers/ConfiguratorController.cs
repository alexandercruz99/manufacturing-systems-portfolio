using Configurator.Core.Models;
using Configurator.Core.Pricing;
using Configurator.Core.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Configurator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfiguratorController : ControllerBase
{
    private readonly ILogger<ConfiguratorController> _logger;

    public ConfiguratorController(ILogger<ConfiguratorController> logger)
    {
        _logger = logger;
    }

    [HttpPost("price")]
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

    [HttpPost("validate")]
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


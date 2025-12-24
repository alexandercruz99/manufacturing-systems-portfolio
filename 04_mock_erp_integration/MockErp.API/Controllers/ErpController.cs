using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using MockErp.API.Models;

namespace MockErp.API.Controllers;

[ApiController]
[Route("erp")]
public class ErpController : ControllerBase
{
    private readonly ILogger<ErpController> _logger;
    private static readonly Regex ConfigurationIdPattern = new(@"^CFG-[a-f0-9]{12}$", RegexOptions.IgnoreCase);

    public ErpController(ILogger<ErpController> logger)
    {
        _logger = logger;
    }

    [HttpPost("orders")]
    public IActionResult CreateOrder([FromBody] ErpOrderRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { error = "Request body is required." });
        }

        var validationErrors = new List<string>();

        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState)
            {
                foreach (var message in error.Value.Errors)
                {
                    validationErrors.Add($"{error.Key}: {message.ErrorMessage}");
                }
            }
        }

        if (!ConfigurationIdPattern.IsMatch(request.ConfigurationId))
        {
            validationErrors.Add("ConfigurationId must match format CFG-xxxxxxxxxxxx (12 hex characters).");
        }

        if (request.Items == null || request.Items.Count == 0)
        {
            validationErrors.Add("Items list cannot be empty.");
        }

        if (validationErrors.Count > 0)
        {
            _logger.LogWarning("Order validation failed for OrderId: {OrderId}. Errors: {Errors}",
                request.OrderId,
                string.Join("; ", validationErrors));

            return BadRequest(new { error = "Validation failed.", details = validationErrors });
        }

        var erpOrderId = $"ERP-{Guid.NewGuid():N}".ToUpperInvariant()[..16];

        _logger.LogInformation(
            "Order accepted. OrderId: {OrderId}, ConfigurationId: {ConfigurationId}, ErpOrderId: {ErpOrderId}, TotalPrice: {TotalPrice}, RequestedShipDate: {RequestedShipDate}",
            request.OrderId,
            request.ConfigurationId,
            erpOrderId,
            request.TotalPrice,
            request.RequestedShipDate);

        return Accepted(new { status = "accepted", erpOrderId = erpOrderId });
    }
}


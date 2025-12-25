using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
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

    /// <summary>
    /// Creates a new order in the ERP system
    /// </summary>
    /// <param name="request">The order request containing order details, items, and pricing information</param>
    /// <returns>An accepted response with the ERP order ID</returns>
    /// <response code="202">Order was successfully accepted</response>
    /// <response code="400">Invalid request data or validation failed</response>
    [HttpPost("orders")]
    [ProducesResponseType(typeof(object), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
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

        // Validate that totalPrice matches expectedExtendedPrice if provided
        if (request.ExpectedExtendedPrice.HasValue)
        {
            var priceDifference = Math.Abs(request.TotalPrice - request.ExpectedExtendedPrice.Value);
            // Allow for small rounding differences (up to 0.01)
            if (priceDifference > 0.01m)
            {
                validationErrors.Add(
                    $"Total price ({request.TotalPrice:F2}) does not match expected extended price from configuration ({request.ExpectedExtendedPrice.Value:F2}). " +
                    $"Difference: {priceDifference:F2}. Please ensure totalPrice matches the extendedPrice from the Configurator API result.");
            }
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
            "Order accepted. OrderId: {OrderId}, ConfigurationId: {ConfigurationId}, ErpOrderId: {ErpOrderId}, TotalPrice: {TotalPrice}, ExpectedExtendedPrice: {ExpectedExtendedPrice}, RequestedShipDate: {RequestedShipDate}",
            request.OrderId,
            request.ConfigurationId,
            erpOrderId,
            request.TotalPrice,
            request.ExpectedExtendedPrice,
            request.RequestedShipDate);

        return Accepted(new { status = "accepted", erpOrderId = erpOrderId });
    }
}


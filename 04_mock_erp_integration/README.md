# Mock ERP Integration API

Mock ERP API for order submission with validation, demonstrating integration patterns and order processing workflows.

## What It Solves

- **Order Validation**: Validates order payloads before ERP submission
- **Integration Testing**: Provides mock endpoint for testing order flows
- **Error Handling**: Returns clear validation errors for invalid orders
- **Order Tracking**: Generates deterministic ERP order IDs for traceability
- **End-to-End Flow**: Demonstrates quote-to-order-to-manufacturing workflow

## Failure → Root Cause → Fix

**Problem:** Orders with missing required fields or invalid data were accepted, causing downstream processing failures.

**Root Cause:** Initial implementation lacked comprehensive validation of order payload structure and business rules.

**Fix:** Implemented validation in `ErpController` to check required fields (orderId, configurationId, items, totalPrice) and validate item structure. Returns 400 Bad Request with detailed error messages for invalid orders.

**Evidence:** See `Controllers/ErpController.cs` validation logic and `order_flow.md` for end-to-end flow documentation.

## Setup

### Prerequisites

- .NET 8.0 SDK or later
- Terminal/Command Prompt

### Run Locally

```bash
# Navigate to API directory
cd 04_mock_erp_integration/MockErp.API

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run
```

**Endpoints:**
- HTTP: http://localhost:5002
- HTTPS: https://localhost:5003
- Swagger UI: http://localhost:5002/swagger

## Configuration

No environment variables or configuration files required for basic operation. See `appsettings.json` for logging configuration.

**Example Request:**
```bash
curl -X POST http://localhost:5002/erp/orders \
  -H "Content-Type: application/json" \
  -d '{
    "orderId": "ORD-2024-001",
    "configurationId": "CFG-a1b2c3d4e5f6",
    "items": [
      {
        "code": "FRAME-001",
        "description": "FanCoil Frame Assembly",
        "quantity": 10
      }
    ],
    "requestedShipDate": "2024-02-15T00:00:00Z",
    "routingFlags": ["ExpressBuild"],
    "totalPrice": 12505.00
  }'
```

**Example Response:**
```json
{
  "status": "accepted",
  "erpOrderId": "ERP-1234567890ABCD"
}
```

## Validation Rules

- **orderId**: Required, must be non-empty string
- **configurationId**: Required, must match format `CFG-*`
- **items**: Required, must be non-empty array
- **totalPrice**: Required, must be positive decimal
- **requestedShipDate**: Optional, must be valid ISO 8601 date

## Tests

No automated tests currently. Manual testing via Swagger UI or curl commands.

## Deployment Notes

- Configure logging levels in `appsettings.json` for production
- Set `ASPNETCORE_ENVIRONMENT` to `Production` for production deployments
- In production, replace mock implementation with actual ERP integration
- Consider adding retry logic and circuit breakers for ERP connectivity

## Limitations / Next Steps

- Mock implementation (does not connect to real ERP)
- No database persistence (orders are not saved)
- No authentication/authorization
- **Next Steps:**
  - Add unit tests for order validation
  - Implement actual ERP integration (SAP, Oracle, etc.)
  - Add database persistence for order history
  - Implement retry logic and error handling for ERP connectivity
  - Add order status tracking and webhooks


# Product Configurator API

RESTful API for configuring and pricing manufacturing products (Coils, Fan Coils, Unit Heaters) with deterministic configuration IDs, validation, and quantity-based pricing.

## What It Solves

- **Request Validation**: Validates product dimensions, materials, options, and quantities before processing
- **400 Error Handling**: Returns clear validation error messages for invalid requests
- **Enum Deserialization**: Handles case-insensitive enum parsing for flexible API usage
- **Deterministic Pricing**: Generates consistent configuration IDs and pricing calculations
- **Logging**: Structured logging for successful configurations and errors

## Failure → Root Cause → Fix

**Problem:** API returned 400 Bad Request when enum values in JSON payload used lowercase formatting (e.g., `"fancoil"` instead of `"FanCoil"`).

**Root Cause:** System.Text.Json default enum converter performed case-sensitive string-to-enum conversion, rejecting valid enum values with different casing.

**Fix:** Implemented `CaseInsensitiveEnumConverter<T>` using `Enum.TryParse` with `ignoreCase: true`. Registered converter in `Program.cs` JSON serializer options. Verified with test requests using lowercase enum values.

**Evidence:** See `JsonConverters/CaseInsensitiveEnumConverter.cs` and `Alexander_Cruz_Application_Failure_Analysis.pdf` in root directory.

## Setup

### Prerequisites

- .NET 8.0 SDK or later
- Terminal/Command Prompt

### Run Locally

```bash
# Navigate to API directory
cd 02_product_configurator_app/Configurator.API

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run
```

**Endpoints:**
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001
- Swagger UI: http://localhost:5000/swagger

## Configuration

No environment variables or configuration files required for basic operation. See `appsettings.json` for logging configuration.

**Example Request:**
```bash
curl -X POST http://localhost:5000/api/configurator/price \
  -H "Content-Type: application/json" \
  -d '{
    "productType": "FanCoil",
    "widthIn": 24.0,
    "heightIn": 18.0,
    "depthIn": 12.0,
    "material": "Copper",
    "options": ["Coating", "StainlessFasteners"],
    "quantity": 10
  }'
```

**Example Response:**
```json
{
  "configurationId": "CFG-a1b2c3d4e5f6",
  "productType": "FanCoil",
  "material": "Copper",
  "unitPrice": 1250.50,
  "extendedPrice": 12505.00,
  "bom": [...],
  "createdAtUtc": "2024-01-15T10:30:00Z"
}
```

## Validation Rules

- **Dimensions**: 6.0 to 120.0 inches (width, height, depth)
- **Quantity**: 1 to 1000 units
- **Options**: At least one option must be specified
- **Product Types**: Coil, FanCoil, UnitHeater (case-insensitive)
- **Materials**: Aluminum, Copper, Steel (case-insensitive)

## Tests

No automated tests currently. Manual testing via Swagger UI or curl commands.

## Deployment Notes

- Configure logging levels in `appsettings.json` for production
- Set `ASPNETCORE_ENVIRONMENT` to `Production` for production deployments
- Consider adding authentication/authorization for production use
- API is stateless and can be horizontally scaled

## Limitations / Next Steps

- No database persistence (configurations are not saved)
- No authentication/authorization
- No rate limiting
- **Next Steps:**
  - Add unit tests for validation and pricing logic
  - Add integration tests for API endpoints
  - Implement database persistence for configuration history
  - Add authentication middleware


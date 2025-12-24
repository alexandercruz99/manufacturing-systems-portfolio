# Maintenance Protocol

## Build and Run Instructions

### Prerequisites

- .NET 8.0 SDK installed on macOS
- Terminal access

### Building the Solution

```bash
cd /Users/alexcruz/Desktop/manufacturing-systems-portfolio
dotnet build ManufacturingSystemsPortfolio.sln
```

### Running the Configurator API

```bash
cd 02_product_configurator_app/Configurator.API
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger: `http://localhost:5000/swagger`

### Running the Mock ERP API

```bash
cd 04_mock_erp_integration/MockErp.API
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5002`
- HTTPS: `https://localhost:5003`
- Swagger: `http://localhost:5002/swagger`

## Code Organization

### Project Structure

```
Configurator.Core/
  Enums/          - ProductType, Material, ConfigOption
  Models/         - ConfiguratorRequest, ConfiguratorResult, LineItem
  Validation/     - Validator class
  Pricing/        - PricingEngine class

Configurator.API/
  Controllers/    - ConfiguratorController
  Program.cs      - Startup and configuration

DocumentGenerator.Core/
  DocumentGenerator.cs - PDF generation logic

MockErp.API/
  Controllers/    - ErpController
  Models/         - ErpOrderRequest, ErpOrderItem
  Program.cs      - Startup and configuration
```

## Modifying Pricing Rules

Pricing rules are defined in `Configurator.Core/Pricing/PricingEngine.cs`:

- **Base Price**: `BasePricePerCubicInch` constant
- **Product Multipliers**: `ProductMultipliers` dictionary
- **Material Multipliers**: `MaterialMultipliers` dictionary
- **Option Adders**: `OptionAdders` dictionary
- **Quantity Discounts**: `QuantityDiscounts` dictionary
- **Minimum Price**: `MinimumPrice` constant

To modify pricing:
1. Edit the constants/dictionaries in `PricingEngine.cs`
2. Rebuild the solution: `dotnet build`
3. Restart the API

## Modifying Validation Rules

Validation rules are defined in `Configurator.Core/Validation/Validator.cs`:

- **Dimension Bounds**: `MinDimension`, `MaxDimension` constants
- **Quantity Bounds**: `MinQuantity`, `MaxQuantity` constants

To modify validation:
1. Edit the constants in `Validator.cs`
2. Rebuild the solution
3. Restart the API

## Adding New Product Types

1. Add enum value to `Configurator.Core/Enums/ProductType.cs`
2. Add multiplier to `ProductMultipliers` dictionary in `PricingEngine.cs`
3. Rebuild and restart

## Adding New Materials

1. Add enum value to `Configurator.Core/Enums/Material.cs`
2. Add multiplier to `MaterialMultipliers` dictionary in `PricingEngine.cs`
3. Rebuild and restart

## Adding New Options

1. Add enum value to `Configurator.Core/Enums/ConfigOption.cs`
2. Add adder to `OptionAdders` dictionary in `PricingEngine.cs`
3. Update `GenerateBom` method in `PricingEngine.cs` to include new option in BOM
4. Rebuild and restart

## PDF Generation

PDFs are generated using QuestPDF library. To modify PDF templates:

1. Edit `DocumentGenerator.Core/DocumentGenerator.cs`
2. Rebuild the solution
3. Test PDF generation

## Logging

Both APIs use structured logging with console and debug providers. Logs include:
- Configuration IDs for successful pricing operations
- Validation errors with details
- Order acceptance with ERP order IDs

To modify logging:
1. Edit `Program.cs` in the respective API project
2. Configure log levels in `appsettings.json` or `appsettings.Development.json`

## Testing Changes

After making changes:

1. Build: `dotnet build ManufacturingSystemsPortfolio.sln`
2. Run Configurator API: `cd 02_product_configurator_app/Configurator.API && dotnet run`
3. Test with curl or Swagger UI
4. Verify configuration IDs are deterministic (same input = same ID)
5. Verify validation rules are enforced
6. Verify pricing calculations are correct

## Dependency Updates

To update NuGet packages:

```bash
dotnet add package <PackageName> --version <Version>
```

Current dependencies:
- `Swashbuckle.AspNetCore` 6.5.0 (Swagger)
- `QuestPDF` 2024.3.10 (PDF generation)

## Troubleshooting

### Port Already in Use

If ports 5000-5003 are in use, modify `Properties/launchSettings.json` in the respective API project.

### Build Errors

1. Ensure .NET 8.0 SDK is installed: `dotnet --version`
2. Restore packages: `dotnet restore`
3. Clean and rebuild: `dotnet clean && dotnet build`

### Configuration ID Mismatch

Configuration IDs are deterministic. If you're getting different IDs for the same input:
1. Verify request fields are exactly the same (including decimal precision)
2. Check that options are in the same order (they are sorted before hashing)


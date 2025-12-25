# Manufacturing Systems Portfolio

A portfolio project demonstrating **Application Analyst** and **Production Support** capabilities through real-world failure diagnosis, request validation, API debugging, and production deployment.

**Key Demonstration:** [Application Failure Analysis PDF](Alexander_Cruz_Application_Failure_Analysis.pdf) - Single-page evidence of debugging and fixing JSON enum deserialization failures in production API.

## What This Portfolio Demonstrates

### Application Failure Analysis
- **Fixed JSON enum deserialization failures** causing 400 errors in production
- **Root cause analysis** of case-sensitive enum parsing
- **Implemented case-insensitive enum converter** with verification
- **Evidence:** See `Alexander_Cruz_Application_Failure_Analysis.pdf`

### Request Validation & Error Handling
1. **Product Configurator API**: Request validation, 400 error handling, enum deserialization fixes
2. **Mock ERP Integration**: Order validation, integration testing, deployment patterns
3. **Document Generator**: Production PDF generation with error handling

### Production Support Skills
- Logging and error tracking
- API debugging and troubleshooting
- Request validation and error messages
- Deployment and configuration management

## Technology Stack

- **.NET 8.0** (macOS compatible)
- **ASP.NET Core Web API**
- **QuestPDF** for PDF generation
- **Swagger/OpenAPI** for API documentation
- **Clean Architecture** (Core + API layers)

## Repository Structure

```
manufacturing-systems-portfolio/
├── 01_legacy-system-analysis/        # Legacy system documentation
├── 02_product_configurator_app/      # Configurator Core + API
├── 03_document_generator/            # PDF document generation
├── 04_mock_erp_integration/          # Mock ERP API
├── 05_system_documentation/          # User guides and protocols
├── 06_future_state_design/           # Architecture and migration plans
└── ManufacturingSystemsPortfolio.sln # Solution file
```

## Prerequisites

- .NET 8.0 SDK installed on macOS
- Terminal access

## Setup Instructions

### Step 0: Terminal Setup

```bash
# Navigate to project directory
cd /Users/alexcruz/Desktop/manufacturing-systems-portfolio

# Restore NuGet packages
dotnet restore ManufacturingSystemsPortfolio.sln

# Build solution
dotnet build ManufacturingSystemsPortfolio.sln
```

## Running the Applications

### Configurator API

```bash
cd 02_product_configurator_app/Configurator.API
dotnet run
```

- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001
- **Swagger**: http://localhost:5000/swagger

### Mock ERP API

```bash
cd 04_mock_erp_integration/MockErp.API
dotnet run
```

- **HTTP**: http://localhost:5002
- **HTTPS**: https://localhost:5003
- **Swagger**: http://localhost:5002/swagger

## Example API Requests

### 1. Price a Configuration

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

**Response:**
```json
{
  "configurationId": "CFG-a1b2c3d4e5f6",
  "productType": "FanCoil",
  "material": "Copper",
  "unitPrice": 1250.50,
  "extendedPrice": 12505.00,
  "bom": [
    {
      "code": "FRAME-001",
      "description": "FanCoil Frame Assembly",
      "qty": 10
    },
    {
      "code": "MAT-001",
      "description": "Copper Core Material",
      "qty": 10
    },
    {
      "code": "OPT-COAT",
      "description": "Protective Coating",
      "qty": 10
    },
    {
      "code": "OPT-SS",
      "description": "Stainless Steel Fasteners",
      "qty": 10
    }
  ],
  "createdAtUtc": "2024-01-15T10:30:00Z"
}
```

### 2. Submit an Order to Mock ERP

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
      },
      {
        "code": "MAT-001",
        "description": "Copper Core Material",
        "quantity": 10
      }
    ],
    "requestedShipDate": "2024-02-15T00:00:00Z",
    "routingFlags": ["ExpressBuild"],
    "totalPrice": 12505.00
  }'
```

**Response:**
```json
{
  "status": "accepted",
  "erpOrderId": "ERP-1234567890ABCD"
}
```

## PDF Document Generation

PDFs are generated using the `DocumentGenerator.Core` library. Example usage:

```csharp
using DocumentGenerator.Core;
using Configurator.Core.Pricing;
using Configurator.Core.Models;

// After getting ConfiguratorResult from PricingEngine
var result = PricingEngine.Price(request);

// Generate sales sheet
DocumentGenerator.GenerateSalesSheet(result, "SalesSheet.pdf");

// Generate plant instructions
DocumentGenerator.GeneratePlantInstructions(result, "PlantInstructions.pdf");
```

PDFs are saved to the specified output path. The documents include:
- **SalesSheet.pdf**: Configuration ID, product details, options, pricing
- **PlantInstructions.pdf**: BOM line items, routing flags, manufacturing instructions

## Product Configuration Details

### Product Types
- **Coil**: Base product (1.0x multiplier)
- **FanCoil**: 1.35x price multiplier
- **UnitHeater**: 1.65x price multiplier

### Materials
- **Aluminum**: Base material (1.0x multiplier)
- **Copper**: 1.85x price multiplier
- **Steel**: 1.25x price multiplier

### Configuration Options
- **None**: No additional options
- **Coating**: +$45.00 per unit
- **StainlessFasteners**: +$28.00 per unit
- **HighEfficiencyFins**: +$125.00 per unit
- **ExpressBuild**: +$200.00 per unit (sets routing flag)

### Quantity Discounts
- 1-4 units: 0% discount
- 5-9 units: 5% discount
- 10-24 units: 10% discount
- 25-49 units: 15% discount
- 50+ units: 20% discount

### Validation Rules
- Dimensions: 6.0 to 120.0 inches (width, height, depth)
- Quantity: 1 to 1000 units
- At least one option must be specified

### Configuration ID Format
Deterministic IDs generated using SHA-256 hash: `CFG-xxxxxxxxxxxx` (12 hex characters)

## Documentation

- **Legacy Analysis**: `01_legacy-system-analysis/` - Overview and failure points of legacy Excel/Word/SAP system
- **User Guide**: `05_system_documentation/user_guide.md` - API usage and examples
- **Maintenance Protocol**: `05_system_documentation/maintenance_protocol.md` - Build, run, and modification instructions
- **Versioning Strategy**: `05_system_documentation/versioning_strategy.md` - API versioning and migration approach
- **Future Architecture**: `06_future_state_design/single_platform_architecture.md` - Unified platform design
- **Migration Strategy**: `06_future_state_design/migration_strategy.md` - 6-month migration plan
- **Order Flow**: `04_mock_erp_integration/order_flow.md` - End-to-end order process documentation

## Business Context

This portfolio maps to manufacturing business-systems roles similar to companies like **Modine Manufacturing**, which design and manufacture heating, ventilation, and air conditioning (HVAC) equipment. The system demonstrates:

- **Product Configurator**: Similar to Modine's coil and unit heater configurators that allow sales to specify dimensions, materials, and options
- **Document Generation**: Automated quote and manufacturing instruction generation (replacing manual Word documents)
- **ERP Integration**: Order submission to manufacturing systems (similar to SAP integration)
- **Deterministic Configuration IDs**: Traceability from quote to order to manufacturing

The architecture follows clean separation of concerns, enabling future enhancements such as database persistence, authentication, cloud deployment, and advanced reporting.

## Build and Test

```bash
# Build entire solution
dotnet build ManufacturingSystemsPortfolio.sln

# Run tests (if test projects added)
dotnet test

# Run Configurator API
cd 02_product_configurator_app/Configurator.API && dotnet run

# Run Mock ERP API (in separate terminal)
cd 04_mock_erp_integration/MockErp.API && dotnet run
```

## License

Portfolio project for demonstration purposes.


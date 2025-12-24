# Exact Terminal Commands for Setup

## Step 0: Terminal Setup (macOS)

```bash
# Navigate to project directory
cd /Users/alexcruz/Desktop/manufacturing-systems-portfolio

# Verify .NET 8.0 is installed
dotnet --version

# Restore NuGet packages
dotnet restore ManufacturingSystemsPortfolio.sln

# Build solution
dotnet build ManufacturingSystemsPortfolio.sln
```

## Step 1: Run Configurator API

```bash
cd 02_product_configurator_app/Configurator.API
dotnet run
```

API will be available at:
- HTTP: http://localhost:5000
- Swagger: http://localhost:5000/swagger

## Step 2: Run Mock ERP API (in separate terminal)

```bash
cd 04_mock_erp_integration/MockErp.API
dotnet run
```

API will be available at:
- HTTP: http://localhost:5002
- Swagger: http://localhost:5002/swagger

## Step 3: Generate Sample PDFs

```bash
cd 03_document_generator/DocumentGenerator.Console
dotnet run
```

PDFs will be generated in `03_document_generator/DocumentGenerator.Console/output/`:
- SalesSheet.pdf
- PlantInstructions.pdf

## Test with curl

### Test Configurator API

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

### Test Mock ERP API

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

## Clean Build

```bash
# Clean solution
dotnet clean ManufacturingSystemsPortfolio.sln

# Restore and rebuild
dotnet restore ManufacturingSystemsPortfolio.sln
dotnet build ManufacturingSystemsPortfolio.sln
```


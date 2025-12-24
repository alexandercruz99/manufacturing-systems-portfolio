# User Guide: Product Configurator System

## Overview

The Product Configurator System provides a RESTful API for configuring and pricing manufacturing products (Coils, Fan Coils, and Unit Heaters). The system generates deterministic configuration IDs, validates inputs, calculates pricing with quantity discounts, and produces PDF documents for sales and manufacturing.

## API Endpoints

### Configurator API (Port 5000/5001)

#### POST /api/configurator/price

Calculates pricing for a product configuration.

**Request Body:**
```json
{
  "productType": "FanCoil",
  "widthIn": 24.0,
  "heightIn": 18.0,
  "depthIn": 12.0,
  "material": "Copper",
  "options": ["Coating", "StainlessFasteners"],
  "quantity": 10
}
```

**Response (200 OK):**
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

**Error Response (400 Bad Request):**
```json
{
  "error": "Validation failed.",
  "details": [
    "Width must be between 6.0 and 120.0 inches.",
    "Quantity must be between 1 and 1000."
  ]
}
```

#### POST /api/configurator/validate

Validates a configuration request without calculating pricing.

**Request Body:** Same as `/price` endpoint

**Response (200 OK):**
```json
{
  "message": "Request is valid."
}
```

**Error Response (400 Bad Request):** Same format as `/price` endpoint

### Mock ERP API (Port 5002/5003)

#### POST /erp/orders

Submits an order to the mock ERP system.

**Request Body:**
```json
{
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
}
```

**Response (202 Accepted):**
```json
{
  "status": "accepted",
  "erpOrderId": "ERP-1234567890ABCD"
}
```

**Error Response (400 Bad Request):**
```json
{
  "error": "Validation failed.",
  "details": [
    "ConfigurationId must match format CFG-xxxxxxxxxxxx (12 hex characters).",
    "Items list cannot be empty."
  ]
}
```

## Product Types

- **Coil**: Base product type, standard pricing
- **FanCoil**: 35% price multiplier
- **UnitHeater**: 65% price multiplier

## Materials

- **Aluminum**: Base material, standard pricing
- **Copper**: 85% price multiplier
- **Steel**: 25% price multiplier

## Configuration Options

- **None**: No additional options
- **Coating**: +$45.00 per unit
- **StainlessFasteners**: +$28.00 per unit
- **HighEfficiencyFins**: +$125.00 per unit
- **ExpressBuild**: +$200.00 per unit (also sets routing flag)

## Quantity Discounts

- 1-4 units: 0% discount
- 5-9 units: 5% discount
- 10-24 units: 10% discount
- 25-49 units: 15% discount
- 50+ units: 20% discount

## Validation Rules

- Dimensions: 6.0 to 120.0 inches (width, height, depth)
- Quantity: 1 to 1000 units
- At least one option must be specified (use "None" if no options)

## Configuration ID Format

Configuration IDs are deterministic and generated using SHA-256 hash of normalized request fields. Format: `CFG-xxxxxxxxxxxx` (12 hexadecimal characters).

## Swagger Documentation

Access interactive API documentation at:
- Configurator API: `http://localhost:5000/swagger` or `https://localhost:5001/swagger`
- Mock ERP API: `http://localhost:5002/swagger` or `https://localhost:5003/swagger`

## Example Usage

### Using curl

**Price a configuration:**
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

**Submit an order:**
```bash
curl -X POST http://localhost:5002/erp/orders \
  -H "Content-Type: application/json" \
  -d '{
    "orderId": "ORD-2024-001",
    "configurationId": "CFG-a1b2c3d4e5f6",
    "items": [{"code": "FRAME-001", "description": "FanCoil Frame Assembly", "quantity": 10}],
    "requestedShipDate": "2024-02-15T00:00:00Z",
    "routingFlags": ["ExpressBuild"],
    "totalPrice": 12505.00
  }'
```


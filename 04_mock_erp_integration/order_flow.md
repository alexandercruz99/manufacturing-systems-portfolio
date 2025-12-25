# Order Flow Documentation

## Overview

This document describes the order flow from product configuration through ERP submission in the manufacturing systems portfolio.

## Order Flow Diagram

```
┌─────────────────┐
│  Sales Rep      │
│  (User)         │
└────────┬────────┘
         │
         │ 1. Create Configuration
         ▼
┌─────────────────────────────────────┐
│  Configurator API                   │
│  POST /api/configurator/price        │
│  - Validates dimensions, quantity    │
│  - Calculates pricing                │
│  - Generates ConfigurationId         │
│  - Returns ConfiguratorResult        │
└────────┬────────────────────────────┘
         │
         │ 2. ConfigurationId + Pricing
         ▼
┌─────────────────────────────────────┐
│  Document Generator                  │
│  - GenerateSalesSheet()              │
│  - GeneratePlantInstructions()       │
│  - Output: PDF files                 │
└────────┬────────────────────────────┘
         │
         │ 3. PDFs Generated
         ▼
┌─────────────────────────────────────┐
│  Sales Rep                            │
│  - Reviews configuration              │
│  - Sends quote to customer            │
│  - Receives customer approval         │
└────────┬────────────────────────────┘
         │
         │ 4. Customer Approval
         ▼
┌─────────────────────────────────────┐
│  Mock ERP API                        │
│  POST /erp/orders                    │
│  - Validates order data              │
│  - Validates ConfigurationId format  │
│  - Validates items, pricing           │
│  - Returns ERP Order ID              │
└────────┬────────────────────────────┘
         │
         │ 5. ERP Order ID
         ▼
┌─────────────────────────────────────┐
│  ERP System (SAP)                    │
│  - Order processed                   │
│  - Manufacturing scheduled           │
│  - Status updates                    │
└─────────────────────────────────────┘
```

## Step-by-Step Process

### Step 1: Configuration Creation

**Actor:** Sales Representative

**Action:**
1. Sales rep calls Configurator API with product specifications:
   - Product type (Coil, FanCoil, UnitHeater)
   - Dimensions (width, height, depth)
   - Material (Aluminum, Copper, Steel)
   - Options (Coating, StainlessFasteners, etc.)
   - Quantity

**API Endpoint:**
```
POST /api/configurator/price
```

**Request Example:**
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

**Response:**
- ConfigurationId (deterministic, e.g., "CFG-a1b2c3d4e5f6")
- Unit price and extended price
- Bill of Materials (BOM)
- Created timestamp

### Step 2: Document Generation

**Actor:** System (automated or manual trigger)

**Action:**
1. System generates PDF documents using ConfigurationId and ConfiguratorResult:
   - SalesSheet.pdf: Customer-facing quote document
   - PlantInstructions.pdf: Manufacturing instructions with BOM

**Output:**
- PDF files saved to file system or blob storage
- Documents include ConfigurationId for traceability

### Step 3: Customer Review and Approval

**Actor:** Sales Representative and Customer

**Action:**
1. Sales rep sends SalesSheet.pdf to customer
2. Customer reviews configuration and pricing
3. Customer approves order
4. Sales rep proceeds to order submission

### Step 4: Order Submission to ERP

**Actor:** Sales Representative or Order Entry Clerk

**Action:**
1. Submit order to Mock ERP API with:
   - OrderId (business identifier)
   - ConfigurationId (from Step 1)
   - Items (from BOM)
   - Requested ship date
   - Routing flags (e.g., ExpressBuild)
   - Total price

**API Endpoint:**
```
POST /erp/orders
```

**Request Example:**
```json
{
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
  "totalPrice": 12505.00,
  "expectedExtendedPrice": 12505.00
}
```

**Note:** The `expectedExtendedPrice` field is optional but recommended. If provided, the API will validate that `totalPrice` matches `expectedExtendedPrice` (allowing for small rounding differences up to $0.01). This ensures price consistency between the Configurator API result and the ERP order submission.

**Validation:**
- ConfigurationId format: CFG-xxxxxxxxxxxx (12 hex chars)
- Items list not empty
- Total price >= 0
- If `expectedExtendedPrice` is provided, it must match `totalPrice` (within $0.01 tolerance for rounding)
- Required fields present

**Response:**
- Status: "accepted"
- ErpOrderId: "ERP-1234567890ABCD"

### Step 5: ERP Processing

**Actor:** ERP System (SAP)

**Action:**
1. ERP system receives order
2. Validates against business rules
3. Creates manufacturing order
4. Schedules production
5. Updates order status
6. Notifies stakeholders

## Data Consistency

### ConfigurationId as Key

The ConfigurationId serves as the primary key linking:
- Configuration request
- Pricing calculation
- Generated documents
- ERP order

### Deterministic Generation

ConfigurationId is deterministically generated from request fields:
- Same input = same ConfigurationId
- Enables idempotent operations
- Supports configuration lookup and reuse

## Error Handling

### Configuration Errors

**Validation Failures:**
- Invalid dimensions → 400 Bad Request with error details
- Invalid quantity → 400 Bad Request with error details
- Missing required fields → 400 Bad Request

**Pricing Errors:**
- Calculation exceptions → 500 Internal Server Error
- Logged with ConfigurationId for traceability

### Order Submission Errors

**Validation Failures:**
- Invalid ConfigurationId format → 400 Bad Request
- Empty items list → 400 Bad Request
- Invalid total price → 400 Bad Request

**Business Rule Violations:**
- Documented in error response details
- Logged for analysis

## Logging and Audit Trail

### Configuration Logging

- ConfigurationId logged on successful pricing
- Product type, unit price, extended price logged
- Timestamp recorded

### Order Logging

- OrderId, ConfigurationId, ErpOrderId logged
- Total price, requested ship date logged
- Validation errors logged with details

## Integration Points

### Current State (Portfolio)

- Configurator API → Document Generator (in-process)
- Document Generator → File System (PDF output)
- Mock ERP API → Simulated ERP (returns ERP order ID)

### Future State (Production)

- Configurator API → Document Generator Service
- Document Generator → Blob Storage + Email Service
- Order Management API → SAP ERP (via API/EDI)
- Order Management API → Notification Service

## Example Complete Flow

```bash
# Step 1: Create Configuration
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

# Response includes ConfigurationId: "CFG-a1b2c3d4e5f6"

# Step 2: Generate Documents (programmatic)
# DocumentGenerator.GenerateSalesSheet(result, "SalesSheet.pdf")
# DocumentGenerator.GeneratePlantInstructions(result, "PlantInstructions.pdf")

# Step 4: Submit Order
curl -X POST http://localhost:5002/erp/orders \
  -H "Content-Type: application/json" \
  -d '{
    "orderId": "ORD-2024-001",
    "configurationId": "CFG-a1b2c3d4e5f6",
    "items": [
      {"code": "FRAME-001", "description": "FanCoil Frame Assembly", "quantity": 10},
      {"code": "MAT-001", "description": "Copper Core Material", "quantity": 10}
    ],
    "requestedShipDate": "2024-02-15T00:00:00Z",
    "routingFlags": ["ExpressBuild"],
    "totalPrice": 12505.00,
    "expectedExtendedPrice": 12505.00
  }'

# Response: {"status":"accepted","erpOrderId":"ERP-1234567890ABCD"}
```

## Business Rules

1. **ConfigurationId Format**: Must match CFG-xxxxxxxxxxxx pattern
2. **Price Validation**: If `expectedExtendedPrice` is provided, `totalPrice` must match it (within $0.01 tolerance). This ensures the order price matches the `extendedPrice` from the Configurator API result.
3. **Item Consistency**: Items in order should match BOM from configuration
4. **Routing Flags**: ExpressBuild flag set when ExpressBuild option selected
5. **Ship Date**: Must be in the future

## Future Enhancements

- Real-time order status updates from ERP
- Webhook notifications for order status changes
- Order history and search capabilities
- Batch order submission
- Order modification and cancellation workflows


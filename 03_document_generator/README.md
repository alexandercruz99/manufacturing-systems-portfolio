# Document Generator

PDF generation library for creating sales sheets and plant manufacturing instructions from product configuration data.

## What It Solves

- **Automated Document Generation**: Replaces manual Word document creation with programmatic PDF generation
- **Consistent Formatting**: Ensures all documents follow the same template and structure
- **Business Output**: Generates production-ready sales and manufacturing documents
- **Integration**: Works with Configurator API results to produce complete documentation

## Failure → Root Cause → Fix

**Problem:** Initial PDF generation failed when configuration data contained null or missing fields, causing runtime exceptions.

**Root Cause:** PDF generation code did not handle optional fields or null values gracefully, assuming all configuration data would always be present.

**Fix:** Added null checks and default values for optional fields. Implemented safe string formatting and fallback values for missing data. Added error handling to prevent document generation failures from crashing the application.

**Evidence:** See `DocumentGenerator.Core/DocumentGenerator.cs` for null-safe field handling.

## Setup

### Prerequisites

- .NET 8.0 SDK or later
- QuestPDF library (included via NuGet)

### Run Locally

```bash
# Navigate to console application
cd 03_document_generator/DocumentGenerator.Console

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run (generates sample PDFs)
dotnet run
```

**Output:** PDFs are generated in `DocumentGenerator.Console/output/`:
- `SalesSheet.pdf` - Sales configuration sheet
- `PlantInstructions.pdf` - Manufacturing instructions with BOM

## Configuration

No environment variables required. PDFs are generated using QuestPDF library with embedded fonts.

**Example Usage:**
```csharp
using DocumentGenerator.Core;
using Configurator.Core.Pricing;
using Configurator.Core.Models;

// Get configuration result from PricingEngine
var result = PricingEngine.Price(request);

// Generate sales sheet
DocumentGenerator.GenerateSalesSheet(result, "SalesSheet.pdf");

// Generate plant instructions
DocumentGenerator.GeneratePlantInstructions(result, "PlantInstructions.pdf");
```

## Document Structure

### Sales Sheet
- Configuration ID
- Product type and material
- Options list
- Pricing (unit and extended)
- Timestamp

### Plant Instructions
- Configuration ID
- Product details
- Bill of Materials (BOM) table
- Routing flags
- Manufacturing notes

## Tests

No automated tests currently. Manual verification by generating PDFs and checking output.

## Deployment Notes

- QuestPDF library includes native dependencies for PDF rendering
- Fonts are embedded in the application
- PDF generation is CPU-intensive; consider async processing for high-volume scenarios
- Output directory must be writable by the application

## Limitations / Next Steps

- Fixed template (not configurable)
- No support for custom branding or logos
- No multi-page handling for large BOMs
- **Next Steps:**
  - Add template configuration system
  - Support custom branding and logos
  - Add multi-page support for large documents
  - Implement async PDF generation for better performance
  - Add PDF preview functionality


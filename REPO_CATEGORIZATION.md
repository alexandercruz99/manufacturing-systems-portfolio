# Repository Categorization

## Featured Repos (Application Analyst / Production Support Focus)

### 1. **Configurator API** (`02_product_configurator_app/`)
**Why Featured:**
- ✅ Request validation and 400 error handling
- ✅ Root cause analysis (enum deserialization case-sensitivity)
- ✅ Fix implementation with verification
- ✅ Logging and error handling
- ✅ Production-ready API structure

**60-Second Reviewer Path:**
- **What it does:** REST API for product configuration and pricing with validation
- **Where to look:** `ConfiguratorController.cs` (validation), `Validator.cs` (business rules), `CaseInsensitiveEnumConverter.cs` (fix)
- **How to run:** `cd 02_product_configurator_app/Configurator.API && dotnet run`
- **What proves competency:** Debugging JSON deserialization failures, implementing case-insensitive enum parsing, request validation

### 2. **Mock ERP Integration** (`04_mock_erp_integration/`)
**Why Featured:**
- ✅ Order validation and error handling
- ✅ Integration testing patterns
- ✅ End-to-end order flow documentation
- ✅ Production deployment considerations

**60-Second Reviewer Path:**
- **What it does:** Mock ERP API for order submission with validation
- **Where to look:** `ErpController.cs` (order processing), `order_flow.md` (documentation)
- **How to run:** `cd 04_mock_erp_integration/MockErp.API && dotnet run`
- **What proves competency:** Integration validation, order processing, deployment responsibility

### 3. **Document Generator** (`03_document_generator/`)
**Why Featured:**
- ✅ Production output generation (PDFs)
- ✅ Business document creation
- ✅ Error handling in document generation

**60-Second Reviewer Path:**
- **What it does:** Generates sales sheets and plant instructions from configuration data
- **Where to look:** `DocumentGenerator.cs` (PDF generation logic)
- **How to run:** `cd 03_document_generator/DocumentGenerator.Console && dotnet run`
- **What proves competency:** Production document generation, PDF handling, business output

## Keep (Supporting Documentation)

- `01_legacy-system-analysis/` - Legacy system analysis (demonstrates analytical thinking)
- `05_system_documentation/` - User guides, maintenance protocols (demonstrates documentation skills)
- `06_future_state_design/` - Architecture planning (demonstrates systems thinking)

## Archive/Remove

- `create_pdf/` - Utility script, can be consolidated
- `create_failure_analysis_pdf/` - Utility script, can be consolidated
- `generate_portfolio_overview/` - Utility script, can be consolidated

**Note:** These utility scripts are useful but not core to the Application Analyst story. Consider moving to a `scripts/` or `tools/` folder.


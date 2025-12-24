# Legacy System Overview

## System Architecture

The legacy manufacturing configuration system consists of three disconnected components:

1. **Excel-Based Configurator**: Sales team uses complex Excel spreadsheets with embedded VBA macros to calculate product configurations. These spreadsheets contain hardcoded pricing tables, material multipliers, and option adders that are manually updated by engineering.

2. **Word Document Templates**: After configuration, sales generates Word documents using mail-merge templates. These documents are manually saved to network drives and emailed to customers.

3. **ERP System (SAP)**: Orders are manually entered into SAP by order entry clerks. Configuration data is re-typed from the Word documents, leading to transcription errors and delays.

## Data Flow

```
Sales Rep → Excel Configurator → Word Document → Email to Customer
                                                      ↓
                                            Order Entry Clerk
                                                      ↓
                                                 SAP ERP
```

## Key Characteristics

- **Manual Processes**: No automated integration between systems
- **Version Control Issues**: Multiple versions of Excel spreadsheets exist across sales team
- **Error-Prone**: Manual data entry leads to configuration mismatches
- **Slow Turnaround**: 2-3 day cycle time from configuration to order entry
- **Limited Auditability**: No centralized logging or tracking of configuration changes

## Technology Stack

- Microsoft Excel 2016 with VBA
- Microsoft Word 2016 with mail-merge
- SAP ECC 6.0 (on-premises)
- Windows file shares for document storage

## Business Impact

- **Revenue Loss**: Configuration errors result in rework and customer dissatisfaction
- **Operational Inefficiency**: 40% of order entry time spent on data validation
- **Scalability Constraints**: Cannot support new product types without IT intervention
- **Compliance Risk**: No audit trail for configuration changes


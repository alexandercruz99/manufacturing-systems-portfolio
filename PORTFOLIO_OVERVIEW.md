# Manufacturing Sales & Configuration Systems - Portfolio Overview

## The Problem

Manufacturing sales stacks are fragmented across configuration, pricing, document generation, and ERP systems. Manual handoffs between Excel spreadsheets, Word documents, and SAP create configuration errors, pricing mistakes, and manufacturing rework. Legacy systems are difficult to modify safely, leading to version control issues and inconsistent business rule enforcement. This fragmentation results in delayed quotes, order entry bottlenecks, and lost revenue from preventable errors.

## What Was Built

A rule-based product configurator API that accepts product specifications (dimensions, materials, options, quantity) and returns deterministic pricing with quantity discounts and bill-of-materials. Automated PDF generation produces sales sheets and plant manufacturing instructions from configuration data. A mock ERP integration API validates order payloads and returns ERP order IDs, demonstrating end-to-end traceability from quote to order via deterministic configuration IDs (format: CFG-xxxxxxxxxxxx).

## Why It Matters

• Eliminates manual document generation workflows
• Prevents invalid configurations before ERP entry
• Creates a single source of truth from quote to order
• Enables safer modernization of legacy systems
• Reduces drafting and order-entry workload

## How to Review

• Start with REVIEWER_START_HERE.md
• Run the demo (5 minutes)
• Inspect targeted code areas if desired

## Scope & Intent

This is a portfolio demonstration, not a production deployment. The focus is system design, integration logic, and documentation discipline representative of internal manufacturing systems.


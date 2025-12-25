# Recording Runbook: Manufacturing Systems Portfolio Demo

**Target Runtime:** 3-5 minutes  
**Resolution:** 1080p  
**Format:** Silent screen recording (no narration, no webcam, no music)

---

## 0. PATH Setup (Do This First)

**Before starting the recording, verify dotnet is accessible:**

```powershell
dotnet --version
```

**If you get an error, dotnet is not in PATH. Add it temporarily for the session:**

```powershell
$env:Path += ";C:\Program Files\dotnet"
dotnet --version
```

**Note:** This PATH change only lasts for the current PowerShell session. If you close and reopen terminals, you'll need to run the PATH command again in each new terminal, OR add it permanently to your system PATH (see "Common Failures: Dotnet Not in PATH" section).

---

## 1. Pre-Flight Checklist

- [x] **.NET SDK PATH verified:** Run `dotnet --version` in PowerShell. If error, see "Common Failures: Dotnet Not in PATH" below
- [x] .NET 8.0 SDK installed and verified (✅ Version 10.0.101 detected - compatible)
- [ ] Screen recording software configured for 1080p (OBS, Windows Game Bar, or similar)
- [x ] Two terminal windows prepared (PowerShell or Command Prompt)
- [ x] Browser window ready (Chrome/Edge recommended for Swagger)
- [ x] File Explorer window ready to navigate to PDF output folder
- [ x] Text editor ready to open REVIEWER_START_HERE.md
- [x] Ports 5000 and 5002 confirmed available (✅ Both ports are free)
- [x] Project directory navigated: `C:\Users\Alexw\OneDrive\Desktop\Modine`
- [x] Solution built successfully (✅ Build succeeded with 0 warnings, 0 errors)
- [x ] All browser tabs closed except one clean tab for Swagger

---

## 2. Exact Terminal Commands (In Order)

### Terminal 1: Configurator API

**IMPORTANT:** If `dotnet` is not in PATH, run this first in each terminal:
```powershell
$env:Path += ";C:\Program Files\dotnet"
```

Then run:
```powershell
cd C:\Users\Alexw\OneDrive\Desktop\Modine\02_product_configurator_app\Configurator.API
dotnet run
```

**Expected Output:** API running on `http://localhost:5000`  
**Wait for:** "Now listening on: http://localhost:5000"

### Terminal 2: Mock ERP API

**IMPORTANT:** If `dotnet` is not in PATH, run this first:
```powershell
$env:Path += ";C:\Program Files\dotnet"
```

Then run:
```powershell
cd C:\Users\Alexw\OneDrive\Desktop\Modine\04_mock_erp_integration\MockErp.API
dotnet run
```

**Expected Output:** API running on `http://localhost:5002`  
**Wait for:** "Now listening on: http://localhost:5002"

### Terminal 3: Document Generator (Run After Configurator Response)

**IMPORTANT:** If `dotnet` is not in PATH, run this first:
```powershell
$env:Path += ";C:\Program Files\dotnet"
```

Then run:
```powershell
cd C:\Users\Alexw\OneDrive\Desktop\Modine\03_document_generator\DocumentGenerator.Console
dotnet run
```

**Expected Output:** Console prints ConfigurationId and file paths  
**PDFs Generated:** `03_document_generator\DocumentGenerator.Console\output\SalesSheet.pdf` and `PlantInstructions.pdf`

---

## 3. Exact JSON Payloads

### POST /api/configurator/price

**URL:** `http://localhost:5000/api/configurator/price`  
**Method:** POST  
**Content-Type:** application/json

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

**Expected Response Fields to Highlight:**
- `configurationId` (format: CFG-xxxxxxxxxxxx)
- `unitPrice`
- `extendedPrice`
- `bom` (array with codes, descriptions, quantities)

### POST /erp/orders

**URL:** `http://localhost:5002/erp/orders`  
**Method:** POST  
**Content-Type:** application/json

**IMPORTANT:** Replace `CFG-xxxxxxxxxxxx` with the actual `configurationId` from the Configurator API response. The ConfigurationId format is `CFG-` followed by 12 hexadecimal characters (e.g., `CFG-1a2b3c4d5e6f`). Copy the exact value from the Configurator response - do NOT use the placeholder!

```json
{
  "orderId": "ORD-2024-001",
  "configurationId": "CFG-xxxxxxxxxxxx",
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
    },
    {
      "code": "OPT-COAT",
      "description": "Protective Coating",
      "quantity": 10
    },
    {
      "code": "OPT-SS",
      "description": "Stainless Steel Fasteners",
      "quantity": 10
    }
  ],
  "requestedShipDate": "2024-02-15T00:00:00Z",
  "routingFlags": ["ExpressBuild"],
  "totalPrice": 12505.00
}
```

**Expected Response Fields to Highlight:**
- `status`: "accepted"
- `erpOrderId` (format: ERP-xxxxxxxxxxxxxxxx)

---

## 4. PDF File Paths and Sections to Show

### PDF Location
```
C:\Users\Alexw\OneDrive\Desktop\Modine\03_document_generator\DocumentGenerator.Console\output\
```

### Files to Display
1. **SalesSheet.pdf**
   - Show filename in File Explorer
   - Open PDF and highlight:
     - ConfigurationId (top section)
     - Product details (ProductType, Material, Dimensions)
     - Pricing section (UnitPrice, ExtendedPrice)
     - Options list

2. **PlantInstructions.pdf**
   - Show filename in File Explorer
   - Open PDF and highlight:
     - BOM table (codes, descriptions, quantities)
     - Routing flags section (if ExpressBuild present)

---

## 5. Minute-by-Minute Script

### 0:00 - 0:30 (Setup & Configurator API)

- **0:00-0:10:** Show terminal 1, run Configurator API command
- **0:10-0:20:** Wait for API to start, show "Now listening on: http://localhost:5000"
- **0:20-0:30:** Open browser, navigate to `http://localhost:5000/swagger`
- **PAUSE:** Show Swagger UI loaded

### 0:30 - 1:30 (Configurator Request & Response)

- **0:30-0:45:** Expand POST /api/configurator/price endpoint in Swagger
- **0:45-1:00:** Click "Try it out", paste JSON payload in the request body box (copy from section 3.1 below, or use the compact version: `{"productType":"FanCoil","widthIn":24.0,"heightIn":18.0,"depthIn":12.0,"material":"Copper","options":["Coating","StainlessFasteners"],"quantity":10}`)
- **1:00-1:15:** Click "Execute", wait for response
- **1:15-1:30:** **PAUSE** on response JSON, highlight:
  - `configurationId` (copy this value)
  - `unitPrice`
  - `extendedPrice`
  - `bom` array (scroll to show items)

### 1:30 - 2:00 (Document Generator)

- **1:30-1:40:** Switch to terminal 3, run Document Generator command
- **1:40-1:50:** Show console output with ConfigurationId and file paths
- **1:50-2:00:** Open File Explorer, navigate to output folder
- **PAUSE:** Show both PDF files (SalesSheet.pdf, PlantInstructions.pdf)

### 2:00 - 2:45 (PDF Review)

- **2:00-2:20:** Open SalesSheet.pdf, scroll to show:
  - ConfigurationId at top
  - Product details and pricing
- **2:20-2:40:** Open PlantInstructions.pdf, scroll to show:
  - BOM table with line items
  - Routing flags section
- **2:40-2:45:** Close PDFs, return to browser

### 2:45 - 3:30 (Mock ERP API & Order Submission)

- **2:45-3:00:** Show terminal 2, run Mock ERP API command
- **3:00-3:10:** Wait for API to start, show "Now listening on: http://localhost:5002"
- **3:10-3:20:** Open new tab, navigate to `http://localhost:5002/swagger`
- **3:20-3:30:** Expand POST /erp/orders endpoint
- **PAUSE:** Show endpoint ready

### 3:30 - 4:15 (ERP Order Request & Response)

- **3:30-3:45:** Click "Try it out", paste ERP JSON payload
- **3:45-3:50:** **CRITICAL:** Replace `CFG-xxxxxxxxxxxx` with actual configurationId from Configurator response
- **3:50-4:00:** Click "Execute", wait for response
- **4:00-4:15:** **PAUSE** on response JSON, highlight:
  - `status`: "accepted"
  - `erpOrderId` (format: ERP-xxxxxxxxxxxxxxxx)

### 4:15 - 4:45 (Reviewer Path)

- **4:15-4:25:** Open text editor or markdown viewer
- **4:25-4:35:** Open file: `C:\Users\Alexw\OneDrive\Desktop\Modine\REVIEWER_START_HERE.md`
- **4:35-4:45:** Scroll through document once, showing:
  - Executive Summary section
  - Architecture Diagram
  - Recommended Review Order
  - Where to Look (Technical) section

### 4:45 - 5:00 (Final Pause)

- **4:45-5:00:** **PAUSE** on REVIEWER_START_HERE.md open
- Show document is accessible and contains review guidance
- **END RECORDING**

---

## 6. Common Failures & Fixes

### Dotnet Not in PATH

**Symptom:** `dotnet --version` returns: "The term 'dotnet' is not recognized as the name of a cmdlet, function, script file, or operable program"

**Fix (Temporary - Current Session Only):**
```powershell
# Add dotnet to PATH for current PowerShell session
$env:Path += ";C:\Program Files\dotnet"

# Verify it works
dotnet --version
```

**Fix (Permanent - Add to System PATH):**
```powershell
# Run PowerShell as Administrator, then:
[Environment]::SetEnvironmentVariable("Path", $env:Path + ";C:\Program Files\dotnet", [EnvironmentVariableTarget]::Machine)

# Close and reopen PowerShell, then verify:
dotnet --version
```

**Alternative (If dotnet is in different location):**
```powershell
# Check if dotnet exists in common locations
Test-Path "C:\Program Files\dotnet\dotnet.exe"
Test-Path "$env:ProgramFiles(x86)\dotnet\dotnet.exe"

# If found, add that path instead (replace with actual path)
$env:Path += ";<actual-path-to-dotnet>"
```

**Note:** If dotnet is not installed, download .NET 8.0 SDK from https://dotnet.microsoft.com/download

### Port Already in Use (5000 or 5002)

**Symptom:** Error: "Failed to bind to address http://localhost:5000: address already in use"

**Fix:**
```powershell
# Find process using port 5000
netstat -ano | findstr :5000

# Kill process (replace PID with actual process ID)
taskkill /PID <PID> /F

# Repeat for port 5002 if needed
netstat -ano | findstr :5002
taskkill /PID <PID> /F
```

### Swagger Not Loading

**Symptom:** Browser shows "This site can't be reached" or blank page

**Fixes:**
- Verify API is running (check terminal for "Now listening" message)
- Try `http://localhost:5000/swagger/index.html` explicitly
- Clear browser cache, try incognito mode
- Check firewall isn't blocking localhost
- Verify port matches launchSettings.json (5000 for Configurator, 5002 for ERP)

### PDFs Not Generating

**Symptom:** Document Generator runs but no PDFs in output folder

**Fixes:**
- Verify output folder exists: `03_document_generator\DocumentGenerator.Console\output\`
- Check console output for errors
- Ensure Configurator API response was successful before running generator
- Verify QuestPDF package is restored: `dotnet restore 03_document_generator/DocumentGenerator.Console/DocumentGenerator.Console.csproj`
- Check file permissions on output directory

### ConfigurationId Mismatch in ERP Request

**Symptom:** ERP returns 400 BadRequest: "ConfigurationId must match format CFG-xxxxxxxxxxxx (12 hex characters)"

**Fix:**
- **CRITICAL:** You MUST call the Configurator API first to get a real ConfigurationId
- Copy the exact `configurationId` from Configurator API response (format: `CFG-` followed by 12 lowercase hex characters, e.g., `CFG-1a2b3c4d5e6f`)
- Paste into ERP request JSON, replacing placeholder `CFG-xxxxxxxxxxxx`
- Ensure no extra spaces or characters
- The placeholder `CFG-xxxxxxxxxxxx` will ALWAYS fail validation - it's not a valid format

### Build Errors

**Symptom:** `dotnet run` fails with compilation errors

**Fixes:**
```powershell
# Clean and rebuild
dotnet clean ManufacturingSystemsPortfolio.sln
dotnet restore ManufacturingSystemsPortfolio.sln
dotnet build ManufacturingSystemsPortfolio.sln

# If still failing, check .NET version
dotnet --version
# Should be 8.0.x or higher
```

### APIs Start But Swagger Shows 404

**Symptom:** API runs but `/swagger` endpoint returns 404

**Fixes:**
- Verify `launchSettings.json` has `"launchUrl": "swagger"`
- Check `Program.cs` includes Swagger middleware:
  ```csharp
  app.UseSwagger();
  app.UseSwaggerUI();
  ```
- Try accessing API health endpoint first: `http://localhost:5000/api/configurator/validate` (POST with empty body should return validation error, confirming API is up)

### JSON Deserialization Errors (400 Bad Request)

**Symptom:** API returns 400 with errors like:
- "The request field is required"
- "The JSON value could not be converted to Configurator.Core.Enums.ProductType"

**Cause:** JSON property naming or enum serialization mismatch

**Fix:**
- Ensure JSON uses **camelCase** for property names: `productType`, `widthIn`, `heightIn`, `depthIn`, `material`, `options`, `quantity`
- Ensure enum values use **exact PascalCase** names: `"FanCoil"`, `"Coil"`, `"UnitHeater"` for productType; `"Copper"`, `"Aluminum"`, `"Steel"` for material
- The API is now configured to accept camelCase properties and string-based enum values
- Example correct payload:
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

### JSON Payload Validation Errors

**Symptom:** Configurator API returns 400 with validation errors

**Fixes:**
- Verify all required fields present: `productType`, `widthIn`, `heightIn`, `depthIn`, `material`, `options`, `quantity`
- Check dimension values are between 6.0 and 120.0
- Ensure quantity is between 1 and 1000
- Verify `options` array is not empty
- Check enum values match exactly: 
  - `productType` must be "Coil", "FanCoil", or "UnitHeater"
  - `material` must be "Aluminum", "Copper", or "Steel"
  - `options` must be valid strings: "Coating", "StainlessFasteners", "HighEfficiencyFins", "ExpressBuild" (or "None")

### KeyNotFoundException: Invalid Option Value

**Symptom:** Configurator API returns 500 error: "The given key 'X' was not present in the dictionary" (where X is a number)

**Cause:** Invalid option value in the JSON payload. Options must be valid enum strings, not numbers.

**Fix:**
- Ensure `options` array contains only valid string values: `"Coating"`, `"StainlessFasteners"`, `"HighEfficiencyFins"`, `"ExpressBuild"`
- Do NOT use numeric values (like `6`) - use the string names
- Example correct payload:
  ```json
  {
    "options": ["Coating", "StainlessFasteners"]
  }
  ```
- The validator now catches invalid options and returns a clear error message before processing

---

## 7. Final Export Instructions

### Filename
```
Manufacturing_Systems_Portfolio_Demo.mp4
```

### Export Settings
- **Resolution:** 1920x1080 (1080p)
- **Frame Rate:** 30 fps (or native)
- **Codec:** H.264 (MP4)
- **Bitrate:** 8-12 Mbps (adjust for file size vs quality)

### Quick Verification Steps After Export

1. **Duration Check:** Verify video is 3-5 minutes (180-300 seconds)
2. **Resolution Check:** Play video, verify it's 1080p (check properties or full-screen on 1080p monitor)
3. **Content Verification:**
   - [ ] Configurator API Swagger shows request/response
   - [ ] ConfigurationId visible in response
   - [ ] PDFs shown in File Explorer and opened
   - [ ] ERP API Swagger shows request/response
   - [ ] ErpOrderId visible in response
   - [ ] REVIEWER_START_HERE.md opened and scrolled
4. **Audio Check:** Verify no audio track (silent recording)
5. **File Size:** Should be approximately 50-150 MB for 3-5 minutes at 1080p

### Final Checklist Before Submission

- [ ] Video filename: `Manufacturing_Systems_Portfolio_Demo.mp4`
- [ ] Duration: 3-5 minutes
- [ ] Resolution: 1080p verified
- [ ] All key moments visible (Swagger responses, PDFs, ERP response, Reviewer doc)
- [ ] No narration, no webcam, no music
- [ ] Cursor movements are minimal and deliberate
- [ ] Pauses on proof outputs (configurationId, pricing, PDFs, erpOrderId)
- [ ] End-to-end flow demonstrated: Configurator → PDFs → ERP → Reviewer path

---

## Recording Tips

1. **Cursor Movement:** Move cursor slowly and deliberately. Pause for 2-3 seconds on important elements before clicking.
2. **Pauses:** Hold for 3-5 seconds on:
   - Swagger response JSON (especially configurationId, unitPrice, extendedPrice, bom)
   - PDF filenames in File Explorer
   - PDF content (ConfigurationId, pricing, BOM)
   - ERP response (status, erpOrderId)
   - REVIEWER_START_HERE.md sections
3. **Transitions:** Use smooth window switching. Avoid rapid alt-tabbing.
4. **Zoom:** If text is small, consider zooming browser to 125-150% before recording.
5. **Clean Desktop:** Close unnecessary applications and notifications.
6. **Test Run:** Do one full test recording before final take to verify timing and flow.

---

**End of Runbook**


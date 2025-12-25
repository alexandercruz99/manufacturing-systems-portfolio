using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var document = Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.Letter);
        page.Margin(1.5f, Unit.Centimetre);

        page.Content()
            .Column(column =>
            {
                // Title Section
                column.Item().Text("Application Failure Analysis — Sample")
                    .FontSize(18)
                    .Bold()
                    .AlignLeft();
                
                column.Item().PaddingTop(0.3f, Unit.Centimetre);
                column.Item().Text("Candidate: Alexander Cruz")
                    .FontSize(11);
                column.Item().Text("Role Target: Application Analyst")
                    .FontSize(11);
                
                column.Item().PaddingTop(0.8f, Unit.Centimetre);
                column.Item().LineHorizontal(1f).LineColor(Colors.Grey.Medium);
                column.Item().PaddingTop(0.8f, Unit.Centimetre);

                // Problem Section
                column.Item().Text("1. Problem")
                    .FontSize(12)
                    .Bold();
                column.Item().PaddingTop(0.3f, Unit.Centimetre);
                column.Item().Text("Configurator API POST /api/configurator/price endpoint returned 400 Bad Request with error: \"The JSON value could not be converted to Configurator.Core.Enums.ProductType\". Failure occurred during JSON deserialization in request handling pipeline. Observable symptom: HTTP 400 response with JSON deserialization exception when enum values in request payload used lowercase or mixed-case formatting (e.g., \"fancoil\" instead of \"FanCoil\").")
                    .FontSize(10)
                    .LineHeight(1.3f);

                column.Item().PaddingTop(0.6f, Unit.Centimetre);

                // Root Cause Section
                column.Item().Text("2. Root Cause")
                    .FontSize(12)
                    .Bold();
                column.Item().PaddingTop(0.3f, Unit.Centimetre);
                column.Item().Text("System.Text.Json default enum converter performed case-sensitive string-to-enum conversion. Request payloads with enum values in non-PascalCase format (e.g., \"fancoil\", \"FANCOIL\", \"FanCoil\") failed deserialization despite representing valid enum members. Missing case-insensitive enum conversion configuration in Program.cs JSON serializer options.")
                    .FontSize(10)
                    .LineHeight(1.3f);

                column.Item().PaddingTop(0.6f, Unit.Centimetre);

                // Resolution Section
                column.Item().Text("3. Resolution")
                    .FontSize(12)
                    .Bold();
                column.Item().PaddingTop(0.3f, Unit.Centimetre);
                column.Item().Text("Implemented CaseInsensitiveEnumConverter<T> class using Enum.TryParse with ignoreCase: true parameter. Registered converter factory in Program.cs via JsonSerializerOptions.Converters. Verified fix by sending test requests with lowercase enum values; API now accepts \"fancoil\", \"FANCOIL\", and \"FanCoil\" formats and returns 200 OK with correct pricing calculations.")
                    .FontSize(10)
                    .LineHeight(1.3f);

                column.Item().PaddingTop(0.6f, Unit.Centimetre);

                // Evidence Section
                column.Item().Text("4. Evidence")
                    .FontSize(12)
                    .Bold();
                column.Item().PaddingTop(0.3f, Unit.Centimetre);
                
                // Screenshot 1
                column.Item().Text("Screenshot 1: API Error Response")
                    .FontSize(10)
                    .Bold();
                column.Item().PaddingTop(0.2f, Unit.Centimetre);
                column.Item().Background(Colors.Grey.Lighten3)
                    .Padding(0.3f, Unit.Centimetre)
                    .Text("HTTP 400 response body showing: {\"error\":\"The JSON value 'fancoil' could not be converted to ProductType\"}")
                    .FontSize(9)
                    .FontFamily("Courier");
                column.Item().Text("Proves: Enum deserialization failure with case-sensitive validation")
                    .FontSize(9)
                    .Italic();

                column.Item().PaddingTop(0.4f, Unit.Centimetre);

                // Screenshot 2
                column.Item().Text("Screenshot 2: Fix Implementation")
                    .FontSize(10)
                    .Bold();
                column.Item().PaddingTop(0.2f, Unit.Centimetre);
                column.Item().Background(Colors.Grey.Lighten3)
                    .Padding(0.3f, Unit.Centimetre)
                    .Text("CaseInsensitiveEnumConverter.cs lines 28-31: Enum.TryParse<T>(value, ignoreCase: true, out var result)")
                    .FontSize(9)
                    .FontFamily("Courier");
                column.Item().Text("Proves: Case-insensitive enum parsing logic implemented")
                    .FontSize(9)
                    .Italic();

                column.Item().PaddingTop(0.4f, Unit.Centimetre);

                // Screenshot 3
                column.Item().Text("Screenshot 3: Successful Request After Fix")
                    .FontSize(10)
                    .Bold();
                column.Item().PaddingTop(0.2f, Unit.Centimetre);
                column.Item().Background(Colors.Grey.Lighten3)
                    .Padding(0.3f, Unit.Centimetre)
                    .Text("HTTP 200 response with payload: {\"productType\":\"fancoil\",...} → {\"configurationId\":\"CFG-...\",\"unitPrice\":...}")
                    .FontSize(9)
                    .FontFamily("Courier");
                column.Item().Text("Proves: Lowercase enum values now accepted and processed correctly")
                    .FontSize(9)
                    .Italic();
            });
    });
});

var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Alexander_Cruz_Application_Failure_Analysis.pdf");
document.GeneratePdf(outputPath);
Console.WriteLine($"Created: {outputPath}");


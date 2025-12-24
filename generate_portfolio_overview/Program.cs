using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;

// Ensure UTF-8 encoding
Console.OutputEncoding = Encoding.UTF8;
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

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
                // Page Title
                column.Item().Text("Manufacturing Sales & Configuration Systems - Portfolio Overview")
                    .FontSize(18)
                    .Bold()
                    .AlignCenter();

                column.Item().PaddingVertical(0.8f, Unit.Centimetre);

                // Section 1: The Problem
                column.Item().Text("The Problem")
                    .FontSize(14)
                    .Bold();

                column.Item().PaddingTop(0.3f, Unit.Centimetre);

                column.Item().Text("Manufacturing sales stacks are fragmented across configuration, pricing, document generation, and ERP systems. Manual handoffs between Excel spreadsheets, Word documents, and SAP create configuration errors, pricing mistakes, and manufacturing rework. Legacy systems are difficult to modify safely, leading to version control issues and inconsistent business rule enforcement. This fragmentation results in delayed quotes, order entry bottlenecks, and lost revenue from preventable errors.")
                    .FontSize(11)
                    .LineHeight(1.4f);

                column.Item().PaddingTop(0.5f, Unit.Centimetre);

                // Section 2: What Was Built
                column.Item().Text("What Was Built")
                    .FontSize(14)
                    .Bold();

                column.Item().PaddingTop(0.3f, Unit.Centimetre);

                column.Item().Text("A rule-based product configurator API that accepts product specifications (dimensions, materials, options, quantity) and returns deterministic pricing with quantity discounts and bill-of-materials. Automated PDF generation produces sales sheets and plant manufacturing instructions from configuration data. A mock ERP integration API validates order payloads and returns ERP order IDs, demonstrating end-to-end traceability from quote to order via deterministic configuration IDs (format: CFG-xxxxxxxxxxxx).")
                    .FontSize(11)
                    .LineHeight(1.4f);

                column.Item().PaddingTop(0.5f, Unit.Centimetre);

                // Section 3: Why It Matters
                column.Item().Text("Why It Matters")
                    .FontSize(14)
                    .Bold();

                column.Item().PaddingTop(0.3f, Unit.Centimetre);

                column.Item().PaddingLeft(0.5f, Unit.Centimetre)
                    .Column(bullets =>
                    {
                        bullets.Item().Text("• Eliminates manual document generation workflows")
                            .FontSize(11)
                            .LineHeight(1.3f);
                        bullets.Item().Text("• Prevents invalid configurations before ERP entry")
                            .FontSize(11)
                            .LineHeight(1.3f);
                        bullets.Item().Text("• Creates a single source of truth from quote to order")
                            .FontSize(11)
                            .LineHeight(1.3f);
                        bullets.Item().Text("• Enables safer modernization of legacy systems")
                            .FontSize(11)
                            .LineHeight(1.3f);
                        bullets.Item().Text("• Reduces drafting and order-entry workload")
                            .FontSize(11)
                            .LineHeight(1.3f);
                    });

                column.Item().PaddingTop(0.5f, Unit.Centimetre);

                // Section 4: How to Review
                column.Item().Text("How to Review")
                    .FontSize(14)
                    .Bold();

                column.Item().PaddingTop(0.3f, Unit.Centimetre);

                column.Item().PaddingLeft(0.5f, Unit.Centimetre)
                    .Column(review =>
                    {
                        review.Item().Text("• Start with REVIEWER_START_HERE.md")
                            .FontSize(11)
                            .LineHeight(1.3f);
                        review.Item().Text("• Run the demo (5 minutes)")
                            .FontSize(11)
                            .LineHeight(1.3f);
                        review.Item().Text("• Inspect targeted code areas if desired")
                            .FontSize(11)
                            .LineHeight(1.3f);
                    });

                column.Item().PaddingTop(0.5f, Unit.Centimetre);

                // Section 5: Scope & Intent
                column.Item().Text("Scope & Intent")
                    .FontSize(14)
                    .Bold();

                column.Item().PaddingTop(0.3f, Unit.Centimetre);

                column.Item().Text("This is a portfolio demonstration, not a production deployment. The focus is system design, integration logic, and documentation discipline representative of internal manufacturing systems.")
                    .FontSize(11)
                    .LineHeight(1.4f);
            });
    });
});

var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "PORTFOLIO_OVERVIEW.pdf");
document.GeneratePdf(outputPath);
Console.WriteLine($"Generated: {outputPath}");

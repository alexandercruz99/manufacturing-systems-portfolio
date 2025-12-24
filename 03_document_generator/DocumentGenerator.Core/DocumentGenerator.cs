using Configurator.Core.Enums;
using Configurator.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DocumentGenerator.Core;

public static class DocumentGenerator
{
    public static void GenerateSalesSheet(ConfiguratorResult result, string outputPath)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(2, Unit.Centimetre);

                page.Content()
                    .Column(column =>
                    {
                        column.Item().Text("SALES CONFIGURATION SHEET")
                            .FontSize(24)
                            .Bold()
                            .AlignCenter();

                        column.Item().PaddingVertical(1, Unit.Centimetre);

                        column.Item().Text($"Configuration ID: {result.ConfigurationId}")
                            .FontSize(14)
                            .Bold();

                        column.Item().PaddingTop(0.5f, Unit.Centimetre);

                        column.Item().Text($"Product Type: {result.ProductType}")
                            .FontSize(12);

                        column.Item().Text($"Material: {result.Material}")
                            .FontSize(12);

                        column.Item().PaddingTop(0.5f, Unit.Centimetre);

                        column.Item().Text("Options:")
                            .FontSize(12)
                            .Bold();

                        var optionsText = result.Bom
                            .Where(item => item.Code.StartsWith("OPT-"))
                            .Select(item => item.Description)
                            .DefaultIfEmpty("None")
                            .Aggregate((a, b) => $"{a}, {b}");

                        column.Item().Text(optionsText)
                            .FontSize(11);

                        column.Item().PaddingTop(1, Unit.Centimetre);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Item").Bold();
                                header.Cell().Element(CellStyle).AlignRight().Text("Price").Bold();
                            });

                            table.Cell().Element(CellStyle).Text("Unit Price");
                            table.Cell().Element(CellStyle).AlignRight().Text($"${result.UnitPrice:F2}");

                            table.Cell().Element(CellStyle).Text("Extended Price");
                            table.Cell().Element(CellStyle).AlignRight().Text($"${result.ExtendedPrice:F2}");

                            table.Cell().Element(CellStyle).Text("Created At (UTC)");
                            table.Cell().Element(CellStyle).AlignRight().Text(result.CreatedAtUtc.ToString("yyyy-MM-dd HH:mm:ss"));
                        });
                    });
            });
        });

        document.GeneratePdf(outputPath);
    }

    public static void GeneratePlantInstructions(ConfiguratorResult result, string outputPath)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(2, Unit.Centimetre);

                page.Content()
                    .Column(column =>
                    {
                        column.Item().Text("PLANT MANUFACTURING INSTRUCTIONS")
                            .FontSize(24)
                            .Bold()
                            .AlignCenter();

                        column.Item().PaddingVertical(1, Unit.Centimetre);

                        column.Item().Text($"Configuration ID: {result.ConfigurationId}")
                            .FontSize(14)
                            .Bold();

                        column.Item().PaddingTop(0.5f, Unit.Centimetre);

                        column.Item().Text($"Product Type: {result.ProductType}")
                            .FontSize(12);

                        column.Item().Text($"Material: {result.Material}")
                            .FontSize(12);

                        column.Item().PaddingTop(1, Unit.Centimetre);

                        column.Item().Text("Bill of Materials (BOM):")
                            .FontSize(14)
                            .Bold();

                        column.Item().PaddingTop(0.5f, Unit.Centimetre);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Code").Bold();
                                header.Cell().Element(CellStyle).Text("Description").Bold();
                                header.Cell().Element(CellStyle).AlignRight().Text("Qty").Bold();
                            });

                            foreach (var item in result.Bom)
                            {
                                table.Cell().Element(CellStyle).Text(item.Code);
                                table.Cell().Element(CellStyle).Text(item.Description);
                                table.Cell().Element(CellStyle).AlignRight().Text(item.Qty.ToString());
                            }
                        });

                        column.Item().PaddingTop(1, Unit.Centimetre);

                        var routingFlags = result.Bom
                            .Where(item => item.Code == "OPT-EXP")
                            .Any() ? "EXPRESS BUILD" : "STANDARD BUILD";

                        column.Item().Text("Routing Flags:")
                            .FontSize(14)
                            .Bold();

                        column.Item().PaddingTop(0.3f, Unit.Centimetre);

                        column.Item().Text(routingFlags)
                            .FontSize(12)
                            .Bold()
                            .FontColor(routingFlags == "EXPRESS BUILD" ? Colors.Red.Medium : Colors.Blue.Medium);
                    });
            });
        });

        document.GeneratePdf(outputPath);
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container
            .Border(1)
            .Padding(5)
            .Background(Colors.Grey.Lighten4);
    }
}


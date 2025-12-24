using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
                column.Item().Text("PORTFOLIO OVERVIEW")
                    .FontSize(24)
                    .Bold()
                    .AlignCenter();

                column.Item().PaddingVertical(1f, Unit.Centimetre);

                column.Item().Text("Content to be provided")
                    .FontSize(12)
                    .AlignCenter();
            });
    });
});

var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "PORTFOLIO_OVERVIEW.pdf");
document.GeneratePdf(outputPath);
Console.WriteLine($"Created: {outputPath}");


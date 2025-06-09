using System.IO;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using NetProject.Services.PdfDocuments;
using NetProject.ViewModels;
using Xunit;

public class PdfReportTests
{
    [Fact]
    public void MonthlySummaryPdf_NotEmpty()
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var vm = new MonthlySummaryReportViewModel
        {
            Month = 1,
            Year = 2025,
            Items = {
                new MonthlySummaryItem {
                    CustomerName = "Test", VehicleReg = "ABC123", OrderCount = 2, TotalCost = 250M
                }
            }
        };

        var document = new MonthlySummaryDocument(vm);
        var pdfBytes = document.GeneratePdf(); // Użyj metody GeneratePdf() z Twojej klasy

        Assert.NotNull(pdfBytes);
        Assert.True(pdfBytes.Length > 0);
    }


}

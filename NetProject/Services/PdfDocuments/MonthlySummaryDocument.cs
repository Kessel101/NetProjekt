using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NetProject.ViewModels;
using System.Globalization;

namespace NetProject.Services.PdfDocuments
{
    public class MonthlySummaryDocument : IDocument
    {
        private readonly MonthlySummaryReportViewModel _vm;
        public MonthlySummaryDocument(MonthlySummaryReportViewModel vm) => _vm = vm;

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text($"Podsumowanie napraw – {_vm.Month:00}/{_vm.Year}")
                    .Bold().FontSize(16).AlignCenter();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(2);
                    });

                    table.Header(h =>
                    {
                        h.Cell().Element(Cell).Text("Klient");
                        h.Cell().Element(Cell).Text("Pojazd");
                        h.Cell().Element(Cell).Text("Zlecenia");
                        h.Cell().Element(Cell).Text("Koszt");
                    });

                    foreach (var i in _vm.Items)
                    {
                        table.Cell().Element(Cell).Text(i.CustomerName);
                        table.Cell().Element(Cell).Text(i.VehicleReg);
                        table.Cell().Element(Cell).Text(i.OrderCount.ToString());
                        table.Cell().Element(Cell).Text(i.TotalCost.ToString("C", CultureInfo.CurrentCulture));
                    }

                    table.Footer(f =>
                    {
                        // scal dwie pierwsze kolumny
                        f.Cell().ColumnSpan(2).Element(Cell)
                         .Text("RAZEM");
                        f.Cell().Element(Cell)
                         .Text(_vm.TotalOrders.ToString());
                        f.Cell().Element(Cell)
                         .Text(_vm.GrandTotalCost.ToString("C", CultureInfo.CurrentCulture));
                    });

                    static IContainer Cell(IContainer c) => c.Padding(5);
                });

                page.Footer()
                    .AlignCenter()
                    .Text($"Wygenerowano: {System.DateTime.Now:yyyy-MM-dd HH:mm}");
            });
        }
    }
}
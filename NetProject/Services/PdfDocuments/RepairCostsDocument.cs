using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NetProject.ViewModels;
using System.Globalization;

namespace NetProject.Services.PdfDocuments
{
    public class RepairCostsDocument : IDocument
    {
        private readonly RepairCostReportViewModel _vm;
        public RepairCostsDocument(RepairCostReportViewModel vm) => _vm = vm;

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Raport kosztów napraw")
                    .SemiBold().FontSize(16).AlignCenter();

                page.Content()
                    .Column(col =>
                    {
                        col.Item().Text($"Pojazd: {_vm.VehicleRegistration}");
                        col.Item().Text($"Okres: {(_vm.Month?.ToString("00") ?? "Wszystkie")} / {_vm.Year}");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            // nagłówek
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Data");
                                header.Cell().Element(CellStyle).Text("Zadanie");
                                header.Cell().Element(CellStyle).Text("Robocizna");
                                header.Cell().Element(CellStyle).Text("Części");
                                header.Cell().Element(CellStyle).Text("Razem");
                            });

                            // wiersze
                            foreach (var i in _vm.Items)
                            {
                                table.Cell().Element(CellStyle).Text(i.Date.ToString("yyyy-MM-dd"));
                                table.Cell().Element(CellStyle).Text(i.TaskName);
                                table.Cell().Element(CellStyle).Text(i.LaborCost.ToString("C", CultureInfo.CurrentCulture));
                                table.Cell().Element(CellStyle).Text(i.PartsCost.ToString("C", CultureInfo.CurrentCulture));
                                table.Cell().Element(CellStyle).Text(i.TotalCost.ToString("C", CultureInfo.CurrentCulture));
                            }

                            // stopka
                            table.Footer(footer =>
                            {
                                // scal dwie pierwsze kolumny
                               footer.Cell().ColumnSpan(2).Element(CellStyle)
                                      .Text("SUMA");
                                footer.Cell().Element(CellStyle)
                                      .Text(_vm.TotalLabor.ToString("C", CultureInfo.CurrentCulture));
                                footer.Cell().Element(CellStyle)
                                      .Text(_vm.TotalParts.ToString("C", CultureInfo.CurrentCulture));
                                footer.Cell().Element(CellStyle)
                                      .Text(_vm.GrandTotal.ToString("C", CultureInfo.CurrentCulture));
                            });

                            static IContainer CellStyle(IContainer c) => c.Padding(5);
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Wygenerowano: ").SemiBold();
                        x.Span(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    });
            });
        }
    }
}
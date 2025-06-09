// Controllers/ReportsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetProject.Data;
using NetProject.Services.PdfDocuments;   // <-- nowa przestrzeń nazw
using NetProject.ViewModels;
using QuestPDF.Infrastructure; // <-- dodanie przestrzeni nazw dla QuestPDF
using QuestPDF.Fluent;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetProject.Controllers
{
    [Authorize(Roles = "Recepcjonista,Admin")]
    public class ReportsController : Controller
    {
        private readonly MyAppDbContext _db;
        public ReportsController(MyAppDbContext db) => _db = db;

        // US12: Raport kosztów napraw klienta
        public async Task<IActionResult> RepairCosts(int? vehicleId, int? month, int? year, bool pdf = false)
        {
            var vm = new RepairCostReportViewModel {
                VehicleId = vehicleId,
                Month     = month,
                Year      = year ?? DateTime.UtcNow.Year
            };

            var query = _db.WorkOrders
                .Include(o => o.Vehicle)
                .Include(o => o.ServiceTasks)
                    .ThenInclude(t => t.ServiceTaskParts)
                        .ThenInclude(sp => sp.Part)
                .AsQueryable();

            if (vehicleId.HasValue)
                query = query.Where(o => o.VehicleId == vehicleId.Value);
            if (month.HasValue)
                query = query.Where(o => o.CreatedAt.Month == month.Value && o.CreatedAt.Year == vm.Year);

            var orders = await query.ToListAsync();

            vm.VehicleRegistration = vehicleId.HasValue
                ? orders.Select(o => o.Vehicle.RegistrationNumber).FirstOrDefault()
                : "Wszystkie";

            foreach (var o in orders)
            foreach (var t in o.ServiceTasks)
            {
                var labor = t.LaborCost;
                var parts = t.ServiceTaskParts.Sum(sp => sp.Part.UnitPrice * sp.Quantity);
                vm.Items.Add(new RepairCostItem {
                    Date      = o.CreatedAt,
                    TaskName  = $"{o.Vehicle.RegistrationNumber} – {t.Name}",
                    LaborCost = labor,
                    PartsCost = parts
                });
            }

            ViewBag.Vehicles = new SelectList(await _db.Vehicles.ToListAsync(),
                                              "Id", "RegistrationNumber", vehicleId);

            if (pdf)
            {
                var doc = new RepairCostsDocument(vm);
                // W najnowszej wersji QuestPDF wystarczy wywołać GeneratePdf() bez opakowywania w Document.Create
                var pdfBytes = doc.GeneratePdf();
                return File(pdfBytes, "application/pdf",
                            $"RaportKosztow_{vm.Year}{vm.Month:00}.pdf");
            }

            return View(vm);
        }

        // US13: Podsumowanie miesięczne (klient, pojazd, suma kosztów, liczba zleceń)
        public async Task<IActionResult> MonthlySummary(int month = 0, int year = 0, bool pdf = false)
        {
            month = month == 0 ? DateTime.UtcNow.Month : month;
            year  = year  == 0 ? DateTime.UtcNow.Year  : year;

            var vm = new MonthlySummaryReportViewModel { Month = month, Year = year };

            var orders = await _db.WorkOrders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.ServiceTasks)
                    .ThenInclude(t => t.ServiceTaskParts)
                        .ThenInclude(sp => sp.Part)
                .Where(o => o.CreatedAt.Month == month && o.CreatedAt.Year == year)
                .ToListAsync();

            vm.Items = orders
                .GroupBy(o => new { o.CustomerId, o.VehicleId })
                .Select(g => new MonthlySummaryItem {
                    CustomerName = g.First().Customer.FullName,
                    VehicleReg   = g.First().Vehicle.RegistrationNumber,
                    OrderCount   = g.Count(),
                    TotalCost    = g.Sum(o =>
                        o.ServiceTasks.Sum(t =>
                            t.LaborCost
                          + t.ServiceTaskParts.Sum(sp => sp.Part.UnitPrice * sp.Quantity)))
                })
                .ToList();

            ViewBag.Months = Enumerable.Range(1, 12)
                .Select(m => new SelectListItem(m.ToString(), m.ToString(), m == month))
                .ToList();
            ViewBag.Years = Enumerable.Range(year - 5, 6)
                .Select(y => new SelectListItem(y.ToString(), y.ToString(), y == year))
                .ToList();

            if (pdf)
            {
                var doc = new MonthlySummaryDocument(vm);
                var pdfBytes = doc.GeneratePdf();
                return File(pdfBytes, "application/pdf",
                            $"Podsumowanie_{vm.Year}{vm.Month:00}.pdf");
            }

            return View(vm);
        }
    }
}

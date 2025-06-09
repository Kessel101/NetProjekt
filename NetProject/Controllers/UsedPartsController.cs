using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetProject.Data;
using NetProject.Models;
using NetProject.ViewModels;

namespace NetProject.Controllers
{
    [Authorize(Roles = "Mechanik,Admin")]
    public class UsedPartsController : Controller
    {
        private readonly MyAppDbContext _db;
        public UsedPartsController(MyAppDbContext db) => _db = db;

        // GET: /UsedParts/Create?serviceTaskId=5
        [HttpGet]
        public IActionResult Create(int serviceTaskId)
        {
            var vm = new UsedPartViewModel
            {
                ServiceTaskId = serviceTaskId,
                Parts = _db.Parts.ToList()
            };
            return View(vm);
        }

        // POST: /UsedParts/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsedPartViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Parts = _db.Parts.ToList();
                return View(vm);
            }

            var part = await _db.Parts.FindAsync(vm.PartId);

            var used = new UsedPart
            {
                ServiceTaskId = vm.ServiceTaskId,
                PartId = vm.PartId,
                Quantity = vm.Quantity
            };
            _db.UsedParts.Add(used);
            await _db.SaveChangesAsync();

            // po dodaniu przekieruj z powrotem do szczegółów zlecenia
            var task = await _db.ServiceTasks.FindAsync(vm.ServiceTaskId);
            return RedirectToAction("Details", "WorkOrders", new { id = task!.WorkOrderId });
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetProject.Data;
using NetProject.Models;
using NetProject.ViewModels;

namespace NetProject.Controllers
{
    [Authorize(Roles = "Mechanik,Admin")]
    public class ServiceTasksController : Controller
    {
        private readonly MyAppDbContext _db;
        public ServiceTasksController(MyAppDbContext db) => _db = db;

        // GET: /ServiceTasks/Create?workOrderId=5
        [HttpGet]
        public IActionResult Create(int workOrderId)
        {
            var vm = new ServiceTaskViewModel
            {
                WorkOrderId = workOrderId
            };
            return View(vm);
        }

        // POST: /ServiceTasks/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceTaskViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var task = new ServiceTask
            {
                WorkOrderId = vm.WorkOrderId,
                Name         = vm.Name,
                LaborCost   = vm.LaborCost
            };

            _db.ServiceTasks.Add(task);
            await _db.SaveChangesAsync();

            // Po dodaniu – wracamy do szczegółów zlecenia
            return RedirectToAction("Details", "WorkOrders", new { id = vm.WorkOrderId });
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetProject.Data;
using NetProject.Models;
using NetProject.ViewModels;


namespace NetProject.Controllers
{
    [Authorize(Roles = "Recepcjonista,Admin")]
    public class WorkOrdersController : Controller
    {
        private readonly MyAppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkOrdersController(MyAppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var mechanics = await _userManager.GetUsersInRoleAsync("Mechanik");
            var vm = new WorkOrderViewModel
            {
                Customers = _db.Customers.ToList(),
                Vehicles = _db.Vehicles.ToList(),
                Mechanics = mechanics
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkOrderViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Customers = _db.Customers.ToList();
                vm.Vehicles = _db.Vehicles.ToList();
                vm.Mechanics = await _userManager.GetUsersInRoleAsync("Mechanik");
                return View(vm);
            }

            var order = new WorkOrder
            {
                CustomerId = vm.CustomerId,
                VehicleId = vm.VehicleId,
                Description = vm.Description,
                AssignedMechanicId = vm.AssignedMechanicId,
                Status = "Nowe"
            };

            _db.WorkOrders.Add(order);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "WorkOrders");
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var orders = _db.WorkOrders
                .Include(w => w.Customer)
                .Include(w => w.Vehicle)
                .Include(w => w.AssignedMechanic)
                .ToList();

            return View(orders);
        }
    }
    
    
}
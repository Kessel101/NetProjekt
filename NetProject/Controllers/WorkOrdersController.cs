using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetProject.Data;
using NetProject.Models;
using NetProject.ViewModels;

namespace NetProject.Controllers
{
    [Authorize] // Wszyscy zalogowani
    public class WorkOrdersController : Controller
    {
        private readonly MyAppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkOrdersController(MyAppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /WorkOrders
        [Authorize(Roles = "Recepcjonista,Admin,Mechanik")]
        public async Task<IActionResult> Index()
        {
            var orders = await _db.WorkOrders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.AssignedMechanic)
                .ToListAsync();
            return View(orders);
        }

        // GET: /WorkOrders/Details/5
        [Authorize(Roles = "Recepcjonista,Admin,Mechanik")]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _db.WorkOrders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.AssignedMechanic)
                .Include(w => w.ServiceTasks)
                    .ThenInclude(t => t.ServiceTaskParts)
                        .ThenInclude(sp => sp.Part)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET: /WorkOrders/Create
        [Authorize(Roles = "Recepcjonista,Admin")]
        public async Task<IActionResult> Create()
        {
            var mechanics = await _userManager.GetUsersInRoleAsync("Mechanik");
            var vm = new WorkOrderViewModel
            {
                Customers = await _db.Customers.ToListAsync(),
                Vehicles  = await _db.Vehicles.ToListAsync(),
                Mechanics = mechanics
            };
            return View(vm);
        }

        // POST: /WorkOrders/Create
        [Authorize(Roles = "Recepcjonista,Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkOrderViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Customers = await _db.Customers.ToListAsync();
                vm.Vehicles  = await _db.Vehicles.ToListAsync();
                vm.Mechanics = await _userManager.GetUsersInRoleAsync("Mechanik");
                return View(vm);
            }

            var order = new WorkOrder
            {
                CustomerId         = vm.CustomerId,
                VehicleId          = vm.VehicleId,
                Description        = vm.Description,
                AssignedMechanicId = vm.AssignedMechanicId,
                Status             = "Nowe",
                CreatedAt          = DateTime.UtcNow
            };

            _db.WorkOrders.Add(order);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /WorkOrders/Edit/5
        // dostępne dla Recepcjonista, Admin, Mechanik
        [Authorize(Roles = "Recepcjonista,Admin,Mechanik")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _db.WorkOrders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            var mechanics = await _userManager.GetUsersInRoleAsync("Mechanik");

            var vm = new WorkOrderViewModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                VehicleId = order.VehicleId,
                Description = order.Description,
                AssignedMechanicId = order.AssignedMechanicId,
                Status = order.Status,
                Customers = await _db.Customers.ToListAsync(),
                Vehicles = await _db.Vehicles.ToListAsync(),
                Mechanics = mechanics,
                // AvailableStatuses już jest ustawione w konstruktorze modelu
            };

            return View(vm);
        }

        // POST: /WorkOrders/Edit/5
        [Authorize(Roles = "Recepcjonista,Admin,Mechanik")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WorkOrderViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Customers = await _db.Customers.ToListAsync();
                vm.Vehicles  = await _db.Vehicles.ToListAsync();
                vm.Mechanics = await _userManager.GetUsersInRoleAsync("Mechanik");
                return View(vm);
            }

            var order = await _db.WorkOrders.FindAsync(vm.Id);
            if (order == null) return NotFound();

            order.CustomerId         = vm.CustomerId;
            order.VehicleId          = vm.VehicleId;
            order.Description        = vm.Description;
            order.AssignedMechanicId = vm.AssignedMechanicId;
            order.Status             = vm.Status;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

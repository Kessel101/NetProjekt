using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetProject.Data;
using NetProject.Models;
using NetProject.ViewModels;

namespace NetProject.Controllers
{
    [Authorize(Roles = "Recepcjonista,Admin")]
    public class VehiclesController : Controller
    {
        private readonly MyAppDbContext _db;

        public VehiclesController(MyAppDbContext db)
            => _db = db;

        // GET /Vehicles/Create/{customerId}
        [HttpGet]
        public IActionResult Create(int customerId)
        {
            var vm = new VehicleViewModel { CustomerId = customerId };
            return View(vm);
        }

        // POST /Vehicles/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var vehicle = new Vehicle
            {
                Make               = vm.Make,
                Model              = vm.Model,
                VIN                = vm.VIN,
                RegistrationNumber = vm.RegistrationNumber,
                Year               = vm.Year,
                CustomerId         = vm.CustomerId
            };

            _db.Vehicles.Add(vehicle);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Customers", new { id = vm.CustomerId });
        }
    }
}
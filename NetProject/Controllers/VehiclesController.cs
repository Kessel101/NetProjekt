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

            string? imageUrl = null;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                // Generuj unikalną nazwę pliku, np. dodając GUID
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(vm.ImageFile.FileName)}";

                // Ścieżka do zapisu pliku w wwwroot/uploads
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                // Zapis pliku na dysku
                using (var stream = System.IO.File.Create(filePath))
                {
                    await vm.ImageFile.CopyToAsync(stream);
                }

                // Ścieżka do zapisu w bazie (relatywna do wwwroot)
                imageUrl = $"/uploads/{fileName}";
            }

            var vehicle = new Vehicle
            {
                Make = vm.Make,
                Model = vm.Model,
                VIN = vm.VIN,
                RegistrationNumber = vm.RegistrationNumber,
                Year = vm.Year,
                CustomerId = vm.CustomerId,
                ImageUrl = imageUrl
            };

            _db.Vehicles.Add(vehicle);
            Console.WriteLine($"Saving vehicle with ImageUrl: {imageUrl}");
            await _db.SaveChangesAsync();
            Console.WriteLine($"Saved vehicle ID: {vehicle.Id}, ImageUrl: {vehicle.ImageUrl}");
            
            return RedirectToAction("Details", "Customers", new { id = vm.CustomerId });
        }
    }
}
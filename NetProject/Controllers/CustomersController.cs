using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetProject.Data;
using NetProject.Models;
using NetProject.ViewModels;

namespace NetProject.Controllers
{
    [Authorize(Roles = "Recepcjonista,Admin")]
    public class CustomersController : Controller
    {
        private readonly MyAppDbContext _db;
        public CustomersController(MyAppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
            => View(await _db.Customers.ToListAsync());

        // Szczegóły klienta - ładowanie pojazdów włącznie
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _db.Customers
                .Include(c => c.Vehicles)   // dołącz pojazdy
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        public IActionResult Create()
            => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            _db.Customers.Add(new Customer
            {
                FullName = vm.FullName,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber
            });

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
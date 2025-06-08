using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetProject.Data;
using NetProject.Models;

namespace NetProject.Controllers
{
    [Authorize(Roles = "Admin,Receptionist")]
    public class PartsController : Controller
    {
        private readonly MyAppDbContext _db;

        public PartsController(MyAppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Parts.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Part part)
        {
            if (ModelState.IsValid)
            {
                _db.Add(part);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(part);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var part = await _db.Parts.FindAsync(id);
            if (part == null) return NotFound();

            return View(part);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Part part)
        {
            if (id != part.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(part); // Upewnij się, że zawiera Quantity
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.Parts.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(part);
        }

        // ZAMIANA DeleteConfirmed na Delete POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var part = await _db.Parts.FindAsync(id);
            if (part == null) return NotFound();

            _db.Parts.Remove(part);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
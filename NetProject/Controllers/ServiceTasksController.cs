using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetProject.Data;
using NetProject.Models;
using NetProject.ViewModels;
using System.Linq;
using System.Threading.Tasks;

public class ServiceTasksController : Controller
{
    private readonly MyAppDbContext _db;

    public ServiceTasksController(MyAppDbContext db)
    {
        _db = db;
    }

    // GET: ServiceTasks/Create
    public IActionResult Create(int workOrderId)
    {
        var vm = new ServiceTaskViewModel { WorkOrderId = workOrderId };
        return View(vm);
    }

    // POST: ServiceTasks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceTaskViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var serviceTask = new ServiceTask
            {
                Name = vm.Name,
                LaborCost = vm.LaborCost,
                WorkOrderId = vm.WorkOrderId
            };
            _db.ServiceTasks.Add(serviceTask);
            await _db.SaveChangesAsync();
            return RedirectToAction("Details", "WorkOrders", new { id = vm.WorkOrderId });
        }
        return View(vm);
    }

[HttpGet]
public async Task<IActionResult> Edit(int id)
{
    var serviceTask = await _db.ServiceTasks.FindAsync(id);
    if (serviceTask == null)
    {
        return NotFound();
    }

    // Mapowanie encji na ViewModel
    var viewModel = new EditPartInTaskViewModel
    {
        Id = serviceTask.Id,
        Name = serviceTask.Name,
        LaborCost = serviceTask.LaborCost,
        WorkOrderId = serviceTask.WorkOrderId
    };

    return View(viewModel);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, EditPartInTaskViewModel viewModel)
{
    if (id != viewModel.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        // 1. Pobierz oryginalny obiekt z bazy danych
        var serviceTask = await _db.ServiceTasks.FindAsync(id);
        if (serviceTask == null)
        {
            return NotFound();
        }

        // 2. Zaktualizuj właściwości na podstawie danych z ViewModelu
        serviceTask.Name = viewModel.Name;
        serviceTask.LaborCost = viewModel.LaborCost;

        try
        {
            // 3. Zapisz zmiany
            _db.Update(serviceTask);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_db.ServiceTasks.Any(e => e.Id == serviceTask.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        // 4. Przekieruj z powrotem do szczegółów zlecenia nadrzędnego
        return RedirectToAction("Details", "WorkOrders", new { id = serviceTask.WorkOrderId });
    }
    
    // Jeśli walidacja się nie powiodła, zwróć widok z wprowadzonymi danymi
    return View(viewModel);
}

    // GET: ServiceTasks/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var serviceTask = await _db.ServiceTasks.FirstOrDefaultAsync(m => m.Id == id);
        if (serviceTask == null)
        {
            return NotFound();
        }
        return View(serviceTask);
    }

    // POST: ServiceTasks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var serviceTask = await _db.ServiceTasks.FindAsync(id);
        if (serviceTask == null)
        {
            return NotFound();
        }
        
        int workOrderId = serviceTask.WorkOrderId; // Zapisz ID zlecenia przed usunięciem
        
        _db.ServiceTasks.Remove(serviceTask);
        await _db.SaveChangesAsync();
        
        return RedirectToAction("Details", "WorkOrders", new { id = workOrderId });
    }

    // --- Poprawione akcje AddPart ---

    [HttpGet]
    public async Task<IActionResult> AddPart(int serviceTaskId)
    {
        var serviceTask = await _db.ServiceTasks.FindAsync(serviceTaskId);
        if (serviceTask == null)
        {
            return NotFound();
        }

        var viewModel = new AddPartToTaskViewModel
        {
            ServiceTaskId = serviceTask.Id,
            WorkOrderId = serviceTask.WorkOrderId,
            TaskName = serviceTask.Name,
            AvailableParts = new SelectList(_db.Parts.Where(p => p.Quantity > 0), "Id", "Name")
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPart(AddPartToTaskViewModel viewModel)
    {
        var task = await _db.ServiceTasks.FindAsync(viewModel.ServiceTaskId);
        var part = await _db.Parts.FindAsync(viewModel.PartId);

        if (task == null || part == null)
        {
            ModelState.AddModelError("", "Nieprawidłowe zadanie lub część.");
        }
        
        if (part != null && part.Quantity < viewModel.Quantity)
        {
             ModelState.AddModelError("Quantity", $"Niewystarczająca ilość części na stanie (Dostępne: {part.Quantity}).");
        }

        if (ModelState.IsValid)
        {
            var serviceTaskPart = new ServiceTaskPart
            {
                ServiceTaskId = viewModel.ServiceTaskId,
                PartId = viewModel.PartId,
                Quantity = viewModel.Quantity
            };
            
            // Opcjonalnie: Zmniejszanie ilości części w magazynie
            // part.Quantity -= viewModel.Quantity;
            // _db.Update(part);

            _db.ServiceTaskParts.Add(serviceTaskPart);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "WorkOrders", new { id = task.WorkOrderId });
        }

        // Jeśli model jest nieprawidłowy, przygotuj dane dla widoku i zwróć go ponownie
        viewModel.AvailableParts = new SelectList(_db.Parts.Where(p => p.Quantity > 0), "Id", "Name", viewModel.PartId);
        viewModel.TaskName = task?.Name; // Upewnij się, że nazwa zadania jest nadal dostępna
        
        return View(viewModel);
    }
}
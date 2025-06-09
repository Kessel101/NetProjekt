using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetProject.Data;
using NetProject.Models;
using NetProject.ViewModels;

namespace NetProject.Controllers
{
    [Authorize]  // tylko zalogowani mogą komentować
    public class WorkOrderCommentsController : Controller
    {
        private readonly MyAppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkOrderCommentsController(MyAppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkOrderCommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Details", "WorkOrders", new { id = vm.WorkOrderId });

            var comment = new WorkOrderComment
            {
                WorkOrderId = vm.WorkOrderId,
                Content     = vm.Content,
                AuthorId    = _userManager.GetUserId(User)
            };

            _db.WorkOrderComments.Add(comment);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "WorkOrders", new { id = vm.WorkOrderId });
        }
    }
}
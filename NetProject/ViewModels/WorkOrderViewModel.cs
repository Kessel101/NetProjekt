using System.ComponentModel.DataAnnotations;
using NetProject.Models;

namespace NetProject.ViewModels
{
    public class WorkOrderViewModel
    {
        [Required] public int CustomerId { get; set; }

        [Required] public int VehicleId { get; set; }

        [Required] public string Description { get; set; }

        [Required] public string AssignedMechanicId { get; set; }

        public IEnumerable<Customer>? Customers { get; set; }
        public IEnumerable<Vehicle>? Vehicles { get; set; }
        public IEnumerable<ApplicationUser>? Mechanics { get; set; }
    }
}
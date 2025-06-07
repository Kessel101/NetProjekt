using System.ComponentModel.DataAnnotations;
using NetProject.Models;

namespace NetProject.ViewModels
{
    public class WorkOrderViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Klient")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Pojazd")]
        public int VehicleId { get; set; }

        [Required]
        [Display(Name = "Opis problemu")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Mechanik")]
        public string AssignedMechanicId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;

        // Do wypełnienia dropdownów
        public IEnumerable<Customer> Customers { get; set; } = Enumerable.Empty<Customer>();
        public IEnumerable<Vehicle> Vehicles   { get; set; } = Enumerable.Empty<Vehicle>();
        public IEnumerable<ApplicationUser> Mechanics { get; set; } = Enumerable.Empty<ApplicationUser>();
        
        public List<string> AvailableStatuses { get; set; } = new()
        {
            "Nowe",
            "W trakcie",
            "Zakończone",
            "Anulowane"
        };
    }
}
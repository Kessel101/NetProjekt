using System.ComponentModel.DataAnnotations;

namespace NetProject.Models
{
    public class WorkOrder
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Required]
        [Display(Name = "Opis problemu")]
        public string Description { get; set; }

        [Required]
        public string AssignedMechanicId { get; set; }
        public ApplicationUser AssignedMechanic { get; set; }

        [Required]
        public string Status { get; set; } = "Nowe";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
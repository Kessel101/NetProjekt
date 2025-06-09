using System.ComponentModel.DataAnnotations;

namespace NetProject.Models
{
    public class WorkOrder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public string Description { get; set; } = string.Empty;
        public string AssignedMechanicId { get; set; }
        public ApplicationUser AssignedMechanic { get; set; }

        public string Status { get; set; } = "Nowe";
        public DateTime CreatedAt { get; set; }
        public ICollection<ServiceTask> ServiceTasks { get; set; } = new List<ServiceTask>();
        
        public ICollection<WorkOrderComment> Comments { get; set; } = new List<WorkOrderComment>();
    }

}
using System;
using System.Collections.Generic;

namespace NetProject.Models
{
    public class ServiceOrder
    {
        public int Id { get; set; }
        public string Status { get; set; } = "Nowe";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relacja z pojazdem
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        // Relacja z mechanikiem (ApplicationUser)
        public string? AssignedMechanicId { get; set; }
        public ApplicationUser? AssignedMechanic { get; set; }

        // Lista czynności i komentarzy
        public List<ServiceTask> ServiceTasks { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}
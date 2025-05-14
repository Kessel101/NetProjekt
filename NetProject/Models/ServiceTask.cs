using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetProject.Models
{
    public class ServiceTask
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal LaborCost { get; set; }

        // Relacja z zleceniem
        public int ServiceOrderId { get; set; }
        public ServiceOrder ServiceOrder { get; set; }

        public List<UsedPart> UsedParts { get; set; } = new();
    }
}
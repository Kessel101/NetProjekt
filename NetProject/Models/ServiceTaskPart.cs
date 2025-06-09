using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetProject.Models
{
    public class ServiceTaskPart
    {
        public int Id { get; set; }

        [Required]
        public int ServiceTaskId { get; set; }
        public ServiceTask ServiceTask { get; set; }

        [Required]
        public int PartId { get; set; }
        public Part Part { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [NotMapped]
        public decimal TotalCost => (Part?.UnitPrice ?? 0) * Quantity;

    }
}
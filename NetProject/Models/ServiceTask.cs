using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetProject.Models
{
    public class ServiceTask
    {
        public int Id { get; set; }

        public ICollection<ServiceTaskPart> ServiceTaskParts { get; set; } = new List<ServiceTaskPart>();

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal LaborCost { get; set; }

        [ForeignKey("WorkOrder")]
        public int WorkOrderId { get; set; }
        public WorkOrder WorkOrder { get; set; }
    }
}
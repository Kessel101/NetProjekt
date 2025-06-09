using System.ComponentModel.DataAnnotations;

namespace NetProject.Models
{
    public class Part
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;
        
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } 

        public ICollection<ServiceTaskPart> ServiceTaskParts { get; set; }
    }
}
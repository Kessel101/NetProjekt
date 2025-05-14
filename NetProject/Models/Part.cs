using System.ComponentModel.DataAnnotations;

namespace NetProject.Models
{
    public class Part
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }
    }
}
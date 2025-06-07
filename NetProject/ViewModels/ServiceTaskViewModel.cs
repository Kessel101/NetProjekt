using System.ComponentModel.DataAnnotations;

namespace NetProject.ViewModels
{
    public class ServiceTaskViewModel
    {
        [Required]
        public int WorkOrderId { get; set; }

        [Required]
        [Display(Name = "Nazwa czynności")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Koszt robocizny")]
        public decimal LaborCost { get; set; }
    }
}
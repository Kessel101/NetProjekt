using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetProject.Models;

namespace NetProject.ViewModels
{
    public class UsedPartViewModel
    {
        [Required]
        [Display(Name = "Czynność serwisowa")]
        public int ServiceTaskId { get; set; }

        public List<ServiceTask> ServiceTasks { get; set; } = new();

        [Required]
        [Display(Name = "Część")]
        public int PartId { get; set; }

        public List<Part> Parts { get; set; } = new();

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetProject.ViewModels
{
    public class AddPartToTaskViewModel
    {
        public int ServiceTaskId { get; set; }
        public int WorkOrderId { get; set; } // Dodane dla przycisku Anuluj
        public string? TaskName { get; set; }

        [Required(ErrorMessage = "Musisz wybrać część.")]
        [Display(Name = "Dostępne części")]
        public int PartId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Ilość musi być większa niż 0.")]
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }

        public IEnumerable<SelectListItem>? AvailableParts { get; set; }
    }
}
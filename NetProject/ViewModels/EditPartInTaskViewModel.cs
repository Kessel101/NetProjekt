using System.ComponentModel.DataAnnotations;

namespace NetProject.ViewModels
{
    public class EditPartInTaskViewModel
    {
        public int Id { get; set; }

        // Potrzebne do przekierowania z powrotem do właściwego zlecenia
        public int WorkOrderId { get; set; }

        [Required(ErrorMessage = "Nazwa czynności jest wymagana.")]
        [Display(Name = "Nazwa czynności")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Koszt robocizny jest wymagany.")]
        [Range(0, 999999.99, ErrorMessage = "Koszt robocizny musi być wartością nieujemną.")]
        [Display(Name = "Koszt robocizny")]
        public decimal LaborCost { get; set; }
    }
}
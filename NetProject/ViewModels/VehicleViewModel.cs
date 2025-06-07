using System.ComponentModel.DataAnnotations;

namespace NetProject.ViewModels
{
    public class VehicleViewModel
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Marka")]
        public string Make { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Model")]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Display(Name = "VIN")]
        public string VIN { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nr rejestracyjny")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        [Range(1900, 2100)]
        [Display(Name = "Rok produkcji")]
        public int Year { get; set; }
    }
}
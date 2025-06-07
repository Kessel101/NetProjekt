using System.ComponentModel.DataAnnotations;

namespace NetProject.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        public string Make { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        [Required]
        public string VIN { get; set; } = string.Empty;

        [Required]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        public string? ImageUrl { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }

}
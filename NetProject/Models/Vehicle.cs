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

        public string? ImageUrl { get; set; }

        // Relacja: pojazd należy do klienta
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
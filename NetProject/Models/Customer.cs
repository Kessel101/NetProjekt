using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetProject.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        // Relacja: jeden klient może mieć wiele pojazdów
        public List<Vehicle> Vehicles { get; set; } = new();
    }
}
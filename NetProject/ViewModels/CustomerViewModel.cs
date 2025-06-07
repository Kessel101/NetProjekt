using System.ComponentModel.DataAnnotations;

namespace NetProject.ViewModels
{
    public class CustomerViewModel
    {
        [Required(ErrorMessage = "Imię i nazwisko jest wymagane")]
        [Display(Name = "Imię i nazwisko")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon jest wymagany")]
        [Phone(ErrorMessage = "Nieprawidłowy numer telefonu")]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}
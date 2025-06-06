using System.ComponentModel.DataAnnotations;

namespace NetProject.DTOs;

public class RegisterDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    
    [Required]
    [Compare("Password", ErrorMessage = "Hasła muszą się zgadzać.")]
    public string ConfirmPassword { get; set; }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace NetProject.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Autor komentarza (użytkownik)
        public string? AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        // Relacja z zleceniem
        public int ServiceOrderId { get; set; }
        public ServiceOrder ServiceOrder { get; set; }
    }
}
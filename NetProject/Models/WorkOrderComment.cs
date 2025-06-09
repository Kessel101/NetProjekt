using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetProject.Models
{
    public class WorkOrderComment
    {
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; } = default!;

        [Required]
        public int WorkOrderId { get; set; }
        [ForeignKey(nameof(WorkOrderId))]
        public WorkOrder WorkOrder { get; set; } = default!;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
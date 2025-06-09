using System.ComponentModel.DataAnnotations;

namespace NetProject.ViewModels
{
    public class WorkOrderCommentViewModel
    {
        public int WorkOrderId { get; set; }

        [Required, StringLength(1000)]
        [Display(Name = "Komentarz")]
        public string Content { get; set; } = string.Empty;
    }
}
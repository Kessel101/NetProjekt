﻿using System.ComponentModel.DataAnnotations;

namespace NetProject.ViewModels
{
    public class ServiceTaskViewModel
    {
        public int WorkOrderId { get; set; }

        [Required(ErrorMessage = "Nazwa czynności jest wymagana.")]
        [Display(Name = "Nazwa czynności")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Koszt robocizny jest wymagany.")]
        [Range(0, double.MaxValue, ErrorMessage = "Koszt robocizny musi być liczbą nieujemną.")]
        [Display(Name = "Koszt robocizny")]
        public decimal LaborCost { get; set; }
    }
}
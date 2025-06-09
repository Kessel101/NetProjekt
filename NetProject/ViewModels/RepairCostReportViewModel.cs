using System;
using System.Collections.Generic;
using System.Linq;

namespace NetProject.ViewModels
{
    public class RepairCostReportViewModel
    {
        // filtry
        public int? VehicleId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

        // dane do wyświetlenia
        public string VehicleRegistration { get; set; }
        public List<RepairCostItem> Items { get; set; } = new();

        // sumy
        public decimal TotalLabor => Items.Sum(i => i.LaborCost);
        public decimal TotalParts => Items.Sum(i => i.PartsCost);
        public decimal GrandTotal => TotalLabor + TotalParts;
    }

    public class RepairCostItem
    {
        public DateTime Date { get; set; }
        public string TaskName { get; set; }
        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }
        public decimal TotalCost => LaborCost + PartsCost;
    }
}